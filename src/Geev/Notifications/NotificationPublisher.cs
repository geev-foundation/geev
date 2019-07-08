using System;
using System.Linq;
using System.Threading.Tasks;
using Geev.BackgroundJobs;
using Geev.Collections.Extensions;
using Geev.Dependency;
using Geev.Domain.Entities;
using Geev.Domain.Uow;
using Geev.Extensions;
using Geev.Json;
using Geev.Runtime.Session;

namespace Geev.Notifications
{
    /// <summary>
    /// Implements <see cref="INotificationPublisher"/>.
    /// </summary>
    public class NotificationPublisher : GeevServiceBase, INotificationPublisher, ITransientDependency
    {
        public const int MaxUserCountToDirectlyDistributeANotification = 5;

        /// <summary>
        /// Indicates all tenants.
        /// </summary>
        public static int[] AllTenants
        {
            get
            {
                return new[] { NotificationInfo.AllTenantIds.To<int>() };
            }
        }

        /// <summary>
        /// Reference to ABP session.
        /// </summary>
        public IGeevSession GeevSession { get; set; }

        private readonly INotificationStore _store;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly INotificationConfiguration _notificationConfiguration;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IIocResolver _iocResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationPublisher"/> class.
        /// </summary>
        public NotificationPublisher(
            INotificationStore store,
            IBackgroundJobManager backgroundJobManager,
            INotificationConfiguration notificationConfiguration,
            IGuidGenerator guidGenerator,
            IIocResolver iocResolver)
        {
            _store = store;
            _backgroundJobManager = backgroundJobManager;
            _notificationConfiguration = notificationConfiguration;
            _guidGenerator = guidGenerator;
            _iocResolver = iocResolver;
            GeevSession = NullGeevSession.Instance;
        }

        //Create EntityIdentifier includes entityType and entityId.
        [UnitOfWork]
        public virtual async Task PublishAsync(
            string notificationName,
            NotificationData data = null,
            EntityIdentifier entityIdentifier = null,
            NotificationSeverity severity = NotificationSeverity.Info,
            UserIdentifier[] userIds = null,
            UserIdentifier[] excludedUserIds = null,
            int?[] tenantIds = null)
        {
            if (notificationName.IsNullOrEmpty())
            {
                throw new ArgumentException("NotificationName can not be null or whitespace!", "notificationName");
            }

            if (!tenantIds.IsNullOrEmpty() && !userIds.IsNullOrEmpty())
            {
                throw new ArgumentException("tenantIds can be set only if userIds is not set!", "tenantIds");
            }

            if (tenantIds.IsNullOrEmpty() && userIds.IsNullOrEmpty())
            {
                tenantIds = new[] { GeevSession.TenantId };
            }

            var notificationInfo = new NotificationInfo(_guidGenerator.Create())
            {
                NotificationName = notificationName,
                EntityTypeName = entityIdentifier == null ? null : entityIdentifier.Type.FullName,
                EntityTypeAssemblyQualifiedName = entityIdentifier == null ? null : entityIdentifier.Type.AssemblyQualifiedName,
                EntityId = entityIdentifier == null ? null : entityIdentifier.Id.ToJsonString(),
                Severity = severity,
                UserIds = userIds.IsNullOrEmpty() ? null : userIds.Select(uid => uid.ToUserIdentifierString()).JoinAsString(","),
                ExcludedUserIds = excludedUserIds.IsNullOrEmpty() ? null : excludedUserIds.Select(uid => uid.ToUserIdentifierString()).JoinAsString(","),
                TenantIds = tenantIds.IsNullOrEmpty() ? null : tenantIds.JoinAsString(","),
                Data = data == null ? null : data.ToJsonString(),
                DataTypeName = data == null ? null : data.GetType().AssemblyQualifiedName
            };

            await _store.InsertNotificationAsync(notificationInfo);

            await CurrentUnitOfWork.SaveChangesAsync(); //To get Id of the notification

            if (userIds != null && userIds.Length <= MaxUserCountToDirectlyDistributeANotification)
            {
                //We can directly distribute the notification since there are not much receivers
                foreach (var notificationDistributorType in _notificationConfiguration.Distributers)
                {
                    using (var notificationDistributer = _iocResolver.ResolveAsDisposable<INotificationDistributer>(notificationDistributorType))
                    {
                        await notificationDistributer.Object.DistributeAsync(notificationInfo.Id);
                    }
                }
            }
            else
            {
                //We enqueue a background job since distributing may get a long time
                await _backgroundJobManager.EnqueueAsync<NotificationDistributionJob, NotificationDistributionJobArgs>(
                    new NotificationDistributionJobArgs(
                        notificationInfo.Id
                        )
                    );
            }
        }
    }
}