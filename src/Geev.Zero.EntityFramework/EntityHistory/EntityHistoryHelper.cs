using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using Geev.Auditing;
using Geev.Dependency;
using Geev.Domain.Entities;
using Geev.Domain.Entities.Auditing;
using Geev.Domain.Uow;
using Geev.EntityHistory.Extensions;
using Geev.Events.Bus.Entities;
using Geev.Extensions;
using Geev.Json;
using Geev.Runtime.Session;
using Geev.Timing;
using Castle.Core.Logging;
using JetBrains.Annotations;

namespace Geev.EntityHistory
{
    public class EntityHistoryHelper : IEntityHistoryHelper, ITransientDependency
    {
        public ILogger Logger { get; set; }
        public IGeevSession GeevSession { get; set; }
        public IClientInfoProvider ClientInfoProvider { get; set; }
        public IEntityChangeSetReasonProvider EntityChangeSetReasonProvider { get; set; }
        public IEntityHistoryStore EntityHistoryStore { get; set; }

        private readonly IEntityHistoryConfiguration _configuration;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private bool IsEntityHistoryEnabled
        {
            get
            {
                if (!_configuration.IsEnabled)
                {
                    return false;
                }

                if (!_configuration.IsEnabledForAnonymousUsers && (GeevSession?.UserId == null))
                {
                    return false;
                }

                return true;
            }
        }

        public EntityHistoryHelper(
            IEntityHistoryConfiguration configuration,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _configuration = configuration;
            _unitOfWorkManager = unitOfWorkManager;

            GeevSession = NullGeevSession.Instance;
            Logger = NullLogger.Instance;
            ClientInfoProvider = NullClientInfoProvider.Instance;
            EntityChangeSetReasonProvider = NullEntityChangeSetReasonProvider.Instance;
            EntityHistoryStore = NullEntityHistoryStore.Instance;
        }

        public virtual EntityChangeSet CreateEntityChangeSet(DbContext context)
        {
            var changeSet = new EntityChangeSet
            {
                Reason = EntityChangeSetReasonProvider.Reason.TruncateWithPostfix(EntityChangeSet.MaxReasonLength),

                // Fill "who did this change"
                BrowserInfo = ClientInfoProvider.BrowserInfo.TruncateWithPostfix(EntityChangeSet.MaxBrowserInfoLength),
                ClientIpAddress = ClientInfoProvider.ClientIpAddress.TruncateWithPostfix(EntityChangeSet.MaxClientIpAddressLength),
                ClientName = ClientInfoProvider.ComputerName.TruncateWithPostfix(EntityChangeSet.MaxClientNameLength),
                ImpersonatorTenantId = GeevSession.ImpersonatorTenantId,
                ImpersonatorUserId = GeevSession.ImpersonatorUserId,
                TenantId = GeevSession.TenantId,
                UserId = GeevSession.UserId
            };

            if (!IsEntityHistoryEnabled)
            {
                return changeSet;
            }

            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            var relationshipChanges = objectContext.ObjectStateManager
                .GetObjectStateEntries(EntityState.Added | EntityState.Deleted)
                .Where(state => state.IsRelationship)
                .ToList();

            foreach (var entityEntry in context.ChangeTracker.Entries())
            {
                var shouldSaveEntityHistory = ShouldSaveEntityHistory(entityEntry);
                if (!shouldSaveEntityHistory && !entityEntry.HasAuditedProperties())
                {
                    continue;
                }

                var entityType = GetEntityType(objectContext, entityEntry.GetEntityBaseType());
                var entityChange = CreateEntityChange(entityEntry, entityType);
                if (entityChange == null)
                {
                    continue;
                }

                var entitySet = GetEntitySet(objectContext, entityType);
                var propertyChanges = new List<EntityPropertyChange>();
                propertyChanges.AddRange(GetPropertyChanges(entityEntry, entityType, entitySet, shouldSaveEntityHistory));
                propertyChanges.AddRange(GetRelationshipChanges(entityEntry, entityType, entitySet, relationshipChanges, shouldSaveEntityHistory));

                if (!shouldSaveEntityHistory && propertyChanges.Count == 0)
                {
                    continue;
                }

                entityChange.PropertyChanges = propertyChanges;
                changeSet.EntityChanges.Add(entityChange);
            }

            return changeSet;
        }

        public virtual async Task SaveAsync(DbContext context, EntityChangeSet changeSet)
        {
            if (!IsEntityHistoryEnabled)
            {
                return;
            }

            if (changeSet.EntityChanges.Count == 0)
            {
                return;
            }

            UpdateChangeSet(context, changeSet);

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                await EntityHistoryStore.SaveAsync(changeSet);
                await uow.CompleteAsync();
            }
        }

        [CanBeNull]
        private EntityChange CreateEntityChange(DbEntityEntry entityEntry, EntityType entityType)
        {
            EntityChangeType changeType;
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    changeType = EntityChangeType.Created;
                    break;
                case EntityState.Deleted:
                    changeType = EntityChangeType.Deleted;
                    break;
                case EntityState.Modified:
                    changeType = entityEntry.IsDeleted() ? EntityChangeType.Deleted : EntityChangeType.Updated;
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                default:
                    Logger.Error("Unexpected EntityState!");
                    return null;
            }

            var entityId = GetEntityId(entityEntry, entityType);
            if (entityId == null && changeType != EntityChangeType.Created)
            {
                Logger.Error("Unexpected null value for entityId!");
                return null;
            }

            var entityChange = new EntityChange
            {
                ChangeType = changeType,
                EntityEntry = entityEntry, // [NotMapped]
                EntityId = entityId,
                EntityTypeFullName = entityEntry.GetEntityBaseType().FullName,
                TenantId = GeevSession.TenantId
            };

            return entityChange;
        }

        private DateTime GetChangeTime(EntityChange entityChange)
        {
            var entity = entityChange.EntityEntry.As<DbEntityEntry>().Entity;
            switch (entityChange.ChangeType)
            {
                case EntityChangeType.Created:
                    return (entity as IHasCreationTime)?.CreationTime ?? Clock.Now;
                case EntityChangeType.Deleted:
                    return (entity as IHasDeletionTime)?.DeletionTime ?? Clock.Now;
                case EntityChangeType.Updated:
                    return (entity as IHasModificationTime)?.LastModificationTime ?? Clock.Now;
                default:
                    Logger.Error("Unexpected EntityState!");
                    return Clock.Now;
            }
        }

        private EntityType GetEntityType(ObjectContext context, Type entityType, bool useClrType = true)
        {
            var metadataWorkspace = context.MetadataWorkspace;
            if (useClrType)
            {
                /* Get the mapping between Clr types in OSpace */
                var objectItemCollection = ((ObjectItemCollection)metadataWorkspace.GetItemCollection(DataSpace.OSpace));
                return metadataWorkspace
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(e => objectItemCollection.GetClrType(e) == entityType);
            }
            else
            {
                return metadataWorkspace
                    .GetItems<EntityType>(DataSpace.CSpace)
                    .Single(e => e.Name == entityType.Name);
            }
        }

        private EntitySet GetEntitySet(ObjectContext context, EntityType entityType)
        {
            var metadataWorkspace = context.MetadataWorkspace;
            /* Get the mapping between entity set/type in CSpace */
            return metadataWorkspace
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(e => e.ElementType.Name == entityType.Name);
        }

        [CanBeNull]
        private string GetEntityId(DbEntityEntry entityEntry, EntityType entityType)
        {
            var primaryKey = entityType.KeyProperties.First();
            return entityEntry.Property(primaryKey.Name)?.GetNewValue()?.ToJsonString();
        }

        /// <summary>
        /// Gets the property changes for this entry.
        /// </summary>
        private ICollection<EntityPropertyChange> GetPropertyChanges(DbEntityEntry entityEntry, EntityType entityType, EntitySet entitySet, bool shouldSaveEntityHistory)
        {
            var propertyChanges = new List<EntityPropertyChange>();
            var propertyNames = entityType.Properties.Select(e => e.Name);
            var complexTypeProperties = entitySet.ElementType.Properties.Where(e => e.IsComplexType).ToList();
            var isCreated = entityEntry.IsCreated();
            var isDeleted = entityEntry.IsDeleted();

            foreach (var propertyName in propertyNames)
            {
                if (entityType.KeyProperties.Any(m => m.Name == propertyName))
                {
                    continue;
                }

                var memberEntry = entityEntry.Member(propertyName);
                if (!(memberEntry is DbPropertyEntry))
                {
                    continue;
                }

                var propertyEntry = memberEntry as DbPropertyEntry;
                var propertyInfo = propertyEntry.EntityEntry.GetPropertyInfo(propertyEntry.Name);
                if (ShouldSavePropertyHistory(propertyEntry, propertyInfo, complexTypeProperties, shouldSaveEntityHistory, isCreated || isDeleted))
                {
                    propertyChanges.Add(new EntityPropertyChange
                    {
                        NewValue = isDeleted ? null : propertyEntry.GetNewValue().ToJsonString().TruncateWithPostfix(EntityPropertyChange.MaxValueLength),
                        OriginalValue = isCreated ? null : propertyEntry.GetOriginalValue().ToJsonString().TruncateWithPostfix(EntityPropertyChange.MaxValueLength),
                        PropertyName = propertyName,
                        PropertyTypeFullName = propertyInfo.PropertyType.FullName,
                        TenantId = GeevSession.TenantId
                    });
                }
            }

            return propertyChanges;
        }

        /// <summary>
        /// Gets the property changes for this entry.
        /// </summary>
        private ICollection<EntityPropertyChange> GetRelationshipChanges(DbEntityEntry entityEntry, EntityType entityType, EntitySet entitySet, ICollection<ObjectStateEntry> relationshipChanges, bool shouldSaveEntityHistory)
        {
            var propertyChanges = new List<EntityPropertyChange>();
            var navigationProperties = entityType.NavigationProperties;

            var isCreated = entityEntry.IsCreated();
            var isDeleted = entityEntry.IsDeleted();

            // Filter out relationship changes that are irrelevant to current entry
            var entityRelationshipChanges = relationshipChanges
                .Where(change => change.EntitySet is AssociationSet)
                .Where(change => change.EntitySet.As<AssociationSet>()
                    .AssociationSetEnds
                    .Select(set => set.EntitySet.ElementType.FullName).Contains(entitySet.ElementType.FullName)
                )
                .ToList();

            var relationshipGroups = entityRelationshipChanges
                .SelectMany(change =>
                {
                    var values = change.State == EntityState.Added ? change.CurrentValues : change.OriginalValues;
                    var valuesChangeSet = new object[values.FieldCount];
                    values.GetValues(valuesChangeSet);

                    return valuesChangeSet
                        .Select(value => value.As<EntityKey>())
                        .Where(value => value.EntitySetName != entitySet.Name)
                        .Select(value => new Tuple<string, EntityState, EntityKey>(change.EntitySet.Name, change.State, value));
                })
                .GroupBy(t => t.Item1);

            foreach(var relationship in relationshipGroups)
            {
                var relationshipName = relationship.Key;
                var navigationPropertyName = navigationProperties
                    .Where(p => p.RelationshipType.Name == relationshipName)
                    .Select(p => p.Name)
                    .FirstOrDefault();

                if (navigationPropertyName == null)
                {
                    Logger.ErrorFormat("Unable to find navigation property for relationship {0} in entity {1}", relationshipName, entityType.Name);
                    continue;
                }

                var propertyInfo = entityEntry.GetPropertyInfo(navigationPropertyName);
                if (ShouldSaveRelationshipHistory(entityRelationshipChanges, propertyInfo, shouldSaveEntityHistory, isCreated || isDeleted))
                {
                    var addedRelationship = relationship.FirstOrDefault(p => p.Item2 == EntityState.Added);
                    var deletedRelationship = relationship.FirstOrDefault(p => p.Item2 == EntityState.Deleted);
                    var newValue = addedRelationship?.Item3.EntityKeyValues.ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value);
                    var oldValue = deletedRelationship?.Item3.EntityKeyValues.ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value);

                    propertyChanges.Add(new EntityPropertyChange
                    {
                        NewValue = newValue?.ToJsonString().TruncateWithPostfix(EntityPropertyChange.MaxValueLength),
                        OriginalValue = oldValue?.ToJsonString().TruncateWithPostfix(EntityPropertyChange.MaxValueLength),
                        PropertyName = navigationPropertyName,
                        PropertyTypeFullName = propertyInfo.PropertyType.FullName,
                        TenantId = GeevSession.TenantId
                    });
                }
            }

            return propertyChanges;
        }

        private bool ShouldSaveEntityHistory(DbEntityEntry entityEntry)
        {
            if (entityEntry.State == EntityState.Detached ||
                entityEntry.State == EntityState.Unchanged)
            {
                return false;
            }

            if (_configuration.IgnoredTypes.Any(t => t.IsInstanceOfType(entityEntry.Entity)))
            {
                return false;
            }

            var entityType = entityEntry.GetEntityBaseType();
            if (!EntityHelper.IsEntity(entityType))
            {
                return false;
            }

            var shouldSaveEntityHistoryForType = ShouldSaveEntityHistoryForType(entityType);
            if (shouldSaveEntityHistoryForType.HasValue)
            {
                return shouldSaveEntityHistoryForType.Value;
            }

            return false;
        }

        private bool? ShouldSaveEntityHistoryForType(Type entityType)
        {
            if (!entityType.IsPublic)
            {
                return false;
            }

            if (entityType.GetTypeInfo().IsDefined(typeof(DisableAuditingAttribute), true))
            {
                return false;
            }

            if (entityType.GetTypeInfo().IsDefined(typeof(AuditedAttribute), true))
            {
                return true;
            }

            if (_configuration.Selectors.Any(selector => selector.Predicate(entityType)))
            {
                return true;
            }

            return null;
        }

        private bool ShouldSavePropertyHistory(DbPropertyEntry propertyEntry, PropertyInfo propertyInfo, ICollection<EdmProperty> complexTypeProperties, bool shouldSaveEntityHistory, bool defaultValue)
        {
            var shouldSavePropertyHistoryForInfo = ShouldSavePropertyHistoryForInfo(propertyInfo, shouldSaveEntityHistory);
            if (shouldSavePropertyHistoryForInfo.HasValue)
            {
                return shouldSavePropertyHistoryForInfo.Value;
            }

            var isModified = false;
            if (propertyEntry is DbComplexPropertyEntry)
            {
                var complexProperty = complexTypeProperties.Single(t => t.Name == propertyInfo.Name);
                isModified = propertyEntry.As<DbComplexPropertyEntry>().HasChanged(complexProperty);
            }
            else
            {
                isModified = propertyEntry.HasChanged();
            }
            if (isModified)
            {
                return true;
            }

            return defaultValue;
        }

        private bool ShouldSaveRelationshipHistory(ICollection<ObjectStateEntry> relationshipChanges, PropertyInfo propertyInfo, bool shouldSaveEntityHistory, bool defaultValue)
        {
            var shouldSavePropertyHistoryForInfo = ShouldSavePropertyHistoryForInfo(propertyInfo, shouldSaveEntityHistory);
            if (shouldSavePropertyHistoryForInfo.HasValue)
            {
                return shouldSavePropertyHistoryForInfo.Value;
            }

            var isModified = relationshipChanges.Any(change => change.State == EntityState.Added || change.State == EntityState.Deleted);
            if (isModified)
            {
                return true;
            }

            return defaultValue;
        }

        private bool? ShouldSavePropertyHistoryForInfo(PropertyInfo propertyInfo, bool shouldSaveEntityHistory)
        {
            if (propertyInfo != null && propertyInfo.IsDefined(typeof(DisableAuditingAttribute), true))
            {
                return false;
            }

            if (!shouldSaveEntityHistory)
            {
                // Should not save property history if property is not audited
                if (propertyInfo == null || !propertyInfo.IsDefined(typeof(AuditedAttribute), true))
                {
                    return false;
                }
            }

            return null;
        }

        /// <summary>
        /// Updates change time, entity id and foreign keys after SaveChanges is called.
        /// </summary>
        private void UpdateChangeSet(DbContext context, EntityChangeSet changeSet)
        {
            foreach (var entityChange in changeSet.EntityChanges)
            {
                /* Update change time */

                entityChange.ChangeTime = GetChangeTime(entityChange);

                /* Update entity id */

                var entityEntry = entityChange.EntityEntry.As<DbEntityEntry>();
                var entityType = GetEntityType(context.As<IObjectContextAdapter>().ObjectContext, entityEntry.GetEntityBaseType(), useClrType: false);
                entityChange.EntityId = GetEntityId(entityEntry, entityType);

                /* Update foreign keys */

                var foreignKeys = entityType.NavigationProperties;

                foreach (var foreignKey in foreignKeys)
                {
                    foreach (var property in foreignKey.GetDependentProperties())
                    {
                        var propertyEntry = entityEntry.Property(property.Name);
                        var propertyChange = entityChange.PropertyChanges.FirstOrDefault(pc => pc.PropertyName == property.Name);

                        //make sure test case cover post saving (for foreign key update)
                        if (propertyChange == null)
                        {
                            if (propertyEntry.HasChanged())
                            {
                                var propertyInfo = entityEntry.GetPropertyInfo(property.Name);

                                // Add foreign key
                                entityChange.PropertyChanges.Add(new EntityPropertyChange
                                {
                                    NewValue = propertyEntry.CurrentValue.ToJsonString(),
                                    OriginalValue = propertyEntry.OriginalValue.ToJsonString(),
                                    PropertyName = property.Name,
                                    PropertyTypeFullName = propertyInfo.PropertyType.FullName
                                });
                            }

                            continue;
                        }

                        if (propertyChange.OriginalValue == propertyChange.NewValue)
                        {
                            var newValue = propertyEntry.GetNewValue().ToJsonString();
                            if (newValue == propertyChange.NewValue)
                            {
                                // No change
                                entityChange.PropertyChanges.Remove(propertyChange);
                            }
                            else
                            {
                                // Update foreign key
                                propertyChange.NewValue = newValue.TruncateWithPostfix(EntityPropertyChange.MaxValueLength);
                            }
                        }
                    }
                }
            }
        }
    }
}
