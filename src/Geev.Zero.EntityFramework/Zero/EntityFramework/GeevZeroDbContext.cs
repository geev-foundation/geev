using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using Geev.Application.Editions;
using Geev.Application.Features;
using Geev.Auditing;
using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.BackgroundJobs;
using Geev.Domain.Entities;
using Geev.EntityFramework.Extensions;
using Geev.MultiTenancy;
using Geev.Notifications;

namespace Geev.Zero.EntityFramework
{
    /// <summary>
    /// Base DbContext for ABP zero.
    /// Derive your DbContext from this class to have base entities.
    /// </summary>
    public abstract class GeevZeroDbContext<TTenant, TRole, TUser> : GeevZeroCommonDbContext<TRole, TUser>
        where TTenant : GeevTenant<TUser>
        where TRole : GeevRole<TUser>
        where TUser : GeevUser<TUser>
    {
        /// <summary>
        /// Tenants
        /// </summary>
        public virtual IDbSet<TTenant> Tenants { get; set; }

        /// <summary>
        /// Editions.
        /// </summary>
        public virtual IDbSet<Edition> Editions { get; set; }

        /// <summary>
        /// FeatureSettings.
        /// </summary>
        public virtual IDbSet<FeatureSetting> FeatureSettings { get; set; }

        /// <summary>
        /// TenantFeatureSetting.
        /// </summary>
        public virtual IDbSet<TenantFeatureSetting> TenantFeatureSettings { get; set; }

        /// <summary>
        /// EditionFeatureSettings.
        /// </summary>
        public virtual IDbSet<EditionFeatureSetting> EditionFeatureSettings { get; set; }

        /// <summary>
        /// Background jobs.
        /// </summary>
        public virtual IDbSet<BackgroundJobInfo> BackgroundJobs { get; set; }

        /// <summary>
        /// User accounts
        /// </summary>
        public virtual IDbSet<UserAccount> UserAccounts { get; set; }

        protected GeevZeroDbContext()
        {

        }

        protected GeevZeroDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        protected GeevZeroDbContext(DbCompiledModel model)
            : base(model)
        {

        }

        protected GeevZeroDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {

        }

        protected GeevZeroDbContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
        }

        protected GeevZeroDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
        }

        protected GeevZeroDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region BackgroundJobInfo.IX_IsAbandoned_NextTryTime

            modelBuilder.Entity<BackgroundJobInfo>()
                .Property(j => j.IsAbandoned)
                .CreateIndex("IX_IsAbandoned_NextTryTime", 1);

            modelBuilder.Entity<BackgroundJobInfo>()
                .Property(j => j.NextTryTime)
                .CreateIndex("IX_IsAbandoned_NextTryTime", 2);

            #endregion

            #region NotificationSubscriptionInfo.IX_NotificationName_EntityTypeName_EntityId_UserId

            modelBuilder.Entity<NotificationSubscriptionInfo>()
                .Property(ns => ns.NotificationName)
                .CreateIndex("IX_NotificationName_EntityTypeName_EntityId_UserId", 1);

            modelBuilder.Entity<NotificationSubscriptionInfo>()
                .Property(ns => ns.EntityTypeName)
                .CreateIndex("IX_NotificationName_EntityTypeName_EntityId_UserId", 2);

            modelBuilder.Entity<NotificationSubscriptionInfo>()
                .Property(ns => ns.EntityId)
                .CreateIndex("IX_NotificationName_EntityTypeName_EntityId_UserId", 3);

            modelBuilder.Entity<NotificationSubscriptionInfo>()
                .Property(ns => ns.UserId)
                .CreateIndex("IX_NotificationName_EntityTypeName_EntityId_UserId", 4);

            #endregion

            #region UserNotificationInfo.IX_UserId_State_CreationTime

            modelBuilder.Entity<UserNotificationInfo>()
                .Property(un => un.UserId)
                .CreateIndex("IX_UserId_State_CreationTime", 1);

            modelBuilder.Entity<UserNotificationInfo>()
                .Property(un => un.State)
                .CreateIndex("IX_UserId_State_CreationTime", 2);

            modelBuilder.Entity<UserNotificationInfo>()
                .Property(un => un.CreationTime)
                .CreateIndex("IX_UserId_State_CreationTime", 3);

            #endregion

            #region UserLoginAttempt.IX_TenancyName_UserNameOrEmailAddress_Result

            modelBuilder.Entity<UserLoginAttempt>()
                .Property(ula => ula.TenancyName)
                .CreateIndex("IX_TenancyName_UserNameOrEmailAddress_Result", 1);

            modelBuilder.Entity<UserLoginAttempt>()
                .Property(ula => ula.UserNameOrEmailAddress)
                .CreateIndex("IX_TenancyName_UserNameOrEmailAddress_Result", 2);

            modelBuilder.Entity<UserLoginAttempt>()
                .Property(ula => ula.Result)
                .CreateIndex("IX_TenancyName_UserNameOrEmailAddress_Result", 3);

            #endregion

            #region UserLoginAttempt.IX_UserId_TenantId

            modelBuilder.Entity<UserLoginAttempt>()
                .Property(ula => ula.UserId)
                .CreateIndex("IX_UserId_TenantId", 1);

            modelBuilder.Entity<UserLoginAttempt>()
                .Property(ula => ula.TenantId)
                .CreateIndex("IX_UserId_TenantId", 2);

            #endregion

            #region AuditLog.Set_MaxLengths

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.ServiceName)
                .HasMaxLength(AuditLog.MaxServiceNameLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.MethodName)
                .HasMaxLength(AuditLog.MaxMethodNameLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.Parameters)
                .HasMaxLength(AuditLog.MaxParametersLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.ClientIpAddress)
                .HasMaxLength(AuditLog.MaxClientIpAddressLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.ClientName)
                .HasMaxLength(AuditLog.MaxClientNameLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.BrowserInfo)
                .HasMaxLength(AuditLog.MaxBrowserInfoLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.Exception)
                .HasMaxLength(AuditLog.MaxExceptionLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.CustomData)
                .HasMaxLength(AuditLog.MaxCustomDataLength);

            #endregion

            #region Common Indexes

            modelBuilder.Conventions.AddAfter<IndexAttributeConvention>(new IndexingPropertyConvention<ISoftDelete, bool>(m => m.IsDeleted));
            modelBuilder.Conventions.AddAfter<IndexAttributeConvention>(new IndexingPropertyConvention<IMayHaveTenant, int?>(m => m.TenantId));
            modelBuilder.Conventions.AddAfter<IndexAttributeConvention>(new IndexingPropertyConvention<IMustHaveTenant, int>(m => m.TenantId));

            #endregion
        }
    }
}
