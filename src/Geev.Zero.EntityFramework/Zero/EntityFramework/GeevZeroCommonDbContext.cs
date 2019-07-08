using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Geev.Auditing;
using Geev.Authorization;
using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.Configuration;
using Geev.EntityFramework;
using Geev.EntityFramework.Extensions;
using Geev.EntityHistory;
using Geev.Localization;
using Geev.Notifications;
using Geev.Organizations;
using System.Threading;
using System.Threading.Tasks;

namespace Geev.Zero.EntityFramework
{
    public abstract class GeevZeroCommonDbContext<TRole, TUser> : GeevDbContext
        where TRole : GeevRole<TUser>
        where TUser : GeevUser<TUser>
    {
        /// <summary>
        /// Roles.
        /// </summary>
        public virtual IDbSet<TRole> Roles { get; set; }

        /// <summary>
        /// Users.
        /// </summary>
        public virtual IDbSet<TUser> Users { get; set; }

        /// <summary>
        /// User logins.
        /// </summary>
        public virtual IDbSet<UserLogin> UserLogins { get; set; }

        /// <summary>
        /// User login attempts.
        /// </summary>
        public virtual IDbSet<UserLoginAttempt> UserLoginAttempts { get; set; }

        /// <summary>
        /// User roles.
        /// </summary>
        public virtual IDbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// User claims.
        /// </summary>
        public virtual IDbSet<UserClaim> UserClaims { get; set; }

        /// <summary>
        /// Permissions.
        /// </summary>
        public virtual IDbSet<PermissionSetting> Permissions { get; set; }

        /// <summary>
        /// Role permissions.
        /// </summary>
        public virtual IDbSet<RolePermissionSetting> RolePermissions { get; set; }

        /// <summary>
        /// User permissions.
        /// </summary>
        public virtual IDbSet<UserPermissionSetting> UserPermissions { get; set; }

        /// <summary>
        /// Settings.
        /// </summary>
        public virtual IDbSet<Setting> Settings { get; set; }

        /// <summary>
        /// Audit logs.
        /// </summary>
        public virtual IDbSet<AuditLog> AuditLogs { get; set; }

        /// <summary>
        /// Languages.
        /// </summary>
        public virtual IDbSet<ApplicationLanguage> Languages { get; set; }

        /// <summary>
        /// LanguageTexts.
        /// </summary>
        public virtual IDbSet<ApplicationLanguageText> LanguageTexts { get; set; }

        /// <summary>
        /// OrganizationUnits.
        /// </summary>
        public virtual IDbSet<OrganizationUnit> OrganizationUnits { get; set; }

        /// <summary>
        /// UserOrganizationUnits.
        /// </summary>
        public virtual IDbSet<UserOrganizationUnit> UserOrganizationUnits { get; set; }

        /// <summary>
        /// OrganizationUnitRoles.
        /// </summary>
        public virtual IDbSet<OrganizationUnitRole> OrganizationUnitRoles { get; set; }

        /// <summary>
        /// Notifications.
        /// </summary>
        public virtual IDbSet<NotificationInfo> Notifications { get; set; }

        /// <summary>
        /// Tenant notifications.
        /// </summary>
        public virtual IDbSet<TenantNotificationInfo> TenantNotifications { get; set; }

        /// <summary>
        /// User notifications.
        /// </summary>
        public virtual IDbSet<UserNotificationInfo> UserNotifications { get; set; }

        /// <summary>
        /// Notification subscriptions.
        /// </summary>
        public virtual IDbSet<NotificationSubscriptionInfo> NotificationSubscriptions { get; set; }

        /// <summary>
        /// Entity changes.
        /// </summary>
        public virtual IDbSet<EntityChange> EntityChanges { get; set; }

        /// <summary>
        /// Entity change sets.
        /// </summary>
        public virtual IDbSet<EntityChangeSet> EntityChangeSets { get; set; }

        /// <summary>
        /// Entity property changes.
        /// </summary>
        public virtual IDbSet<EntityPropertyChange> EntityPropertyChanges { get; set; }

        public IEntityHistoryHelper EntityHistoryHelper { get; set; }

        /// <summary>
        /// Default constructor.
        /// Do not directly instantiate this class. Instead, use dependency injection!
        /// </summary>
        protected GeevZeroCommonDbContext()
        {

        }

        /// <summary>
        /// Constructor with connection string parameter.
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string or a name in connection strings in configuration file</param>
        protected GeevZeroCommonDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        protected GeevZeroCommonDbContext(DbCompiledModel model)
            : base(model)
        {

        }

        /// <summary>
        /// This constructor can be used for unit tests.
        /// </summary>
        protected GeevZeroCommonDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {

        }

        protected GeevZeroCommonDbContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {

        }

        protected GeevZeroCommonDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected GeevZeroCommonDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        public override int SaveChanges()
        {
            var changeSet = EntityHistoryHelper?.CreateEntityChangeSet(this);

            var result = base.SaveChanges();

            EntityHistoryHelper?.Save(this, changeSet);

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var changeSet = EntityHistoryHelper?.CreateEntityChangeSet(this);

            var result = await base.SaveChangesAsync(cancellationToken);

            if (EntityHistoryHelper != null)
            {
                await EntityHistoryHelper.SaveAsync(this, changeSet);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EntityChange>()
                .HasMany(e => e.PropertyChanges)
                .WithRequired()
                .HasForeignKey(e => e.EntityChangeId);

            #region EntityChange.IX_EntityChangeSetId

            modelBuilder.Entity<EntityChange>()
                .Property(e => e.EntityChangeSetId)
                .CreateIndex("IX_EntityChangeSetId", 1);

            #endregion

            #region EntityChange.IX_EntityTypeFullName_EntityId

            modelBuilder.Entity<EntityChange>()
                .Property(e => e.EntityTypeFullName)
                .CreateIndex("IX_EntityTypeFullName_EntityId", 1);

            modelBuilder.Entity<EntityChange>()
                .Property(e => e.EntityId)
                .CreateIndex("IX_EntityTypeFullName_EntityId", 2);

            #endregion

            modelBuilder.Entity<EntityChangeSet>()
                .HasMany(e => e.EntityChanges)
                .WithRequired()
                .HasForeignKey(e => e.EntityChangeSetId);

            #region EntityChangeSet.IX_TenantId_UserId
            
            modelBuilder.Entity<EntityChangeSet>()
                .Property(e => e.TenantId)
                .CreateIndex("IX_TenantId_UserId", 1);

            modelBuilder.Entity<EntityChangeSet>()
                .Property(e => e.UserId)
                .CreateIndex("IX_TenantId_UserId", 2);

            #endregion

            #region EntityChangeSet.IX_TenantId_CreationTime

            modelBuilder.Entity<EntityChangeSet>()
                .Property(e => e.TenantId)
                .CreateIndex("IX_TenantId_CreationTime", 1);

            modelBuilder.Entity<EntityChangeSet>()
                .Property(e => e.CreationTime)
                .CreateIndex("IX_TenantId_CreationTime", 2);

            #endregion

            #region EntityChangeSet.IX_TenantId_Reason

            modelBuilder.Entity<EntityChangeSet>()
                .Property(e => e.TenantId)
                .CreateIndex("IX_TenantId_Reason", 1);

            modelBuilder.Entity<EntityChangeSet>()
                .Property(e => e.Reason)
                .CreateIndex("IX_TenantId_Reason", 2);

            #endregion

            #region EntityPropertyChange.IX_EntityChangeId

            modelBuilder.Entity<EntityPropertyChange>()
                .Property(e => e.EntityChangeId)
                .CreateIndex("IX_EntityChangeId", 1);

            #endregion

        }
    }
}
