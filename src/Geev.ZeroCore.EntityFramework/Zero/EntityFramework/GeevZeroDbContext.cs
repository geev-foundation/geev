using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Geev.Application.Editions;
using Geev.Application.Features;
using Geev.Auditing;
using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.BackgroundJobs;
using Geev.EntityFramework.Extensions;
using Geev.MultiTenancy;
using Geev.Notifications;

namespace Geev.Zero.EntityFramework
{
    /// <summary>
    /// Base DbContext for ABP zero.
    /// Derive your DbContext from this class to have base entities.
    /// </summary>
    public abstract class GeevZeroDbContext<TTenant, TRole, TUser, TSelf> : GeevZeroCommonDbContext<TRole, TUser, TSelf>
        where TTenant : GeevTenant<TUser>
        where TRole : GeevRole<TUser>
        where TUser : GeevUser<TUser>
        where TSelf : GeevZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        /// <summary>
        /// Tenants
        /// </summary>
        public virtual DbSet<TTenant> Tenants { get; set; }

        /// <summary>
        /// Editions.
        /// </summary>
        public virtual DbSet<Edition> Editions { get; set; }

        /// <summary>
        /// FeatureSettings.
        /// </summary>
        public virtual DbSet<FeatureSetting> FeatureSettings { get; set; }

        /// <summary>
        /// TenantFeatureSetting.
        /// </summary>
        public virtual DbSet<TenantFeatureSetting> TenantFeatureSettings { get; set; }

        /// <summary>
        /// EditionFeatureSettings.
        /// </summary>
        public virtual DbSet<EditionFeatureSetting> EditionFeatureSettings { get; set; }

        /// <summary>
        /// Background jobs.
        /// </summary>
        public virtual DbSet<BackgroundJobInfo> BackgroundJobs { get; set; }

        /// <summary>
        /// User accounts
        /// </summary>
        public virtual DbSet<UserAccount> UserAccounts { get; set; }

        /// <summary>
        /// Notifications.
        /// </summary>
        public virtual DbSet<NotificationInfo> Notifications { get; set; }

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
                .Property(e => e.IsAbandoned)
                .CreateIndex("IX_IsAbandoned_NextTryTime", 1);

            modelBuilder.Entity<BackgroundJobInfo>()
                .Property(e => e.NextTryTime)
                .CreateIndex("IX_IsAbandoned_NextTryTime", 2);

            #endregion

            #region TenantFeatureSetting.IX_TenantId_Name

            modelBuilder.Entity<TenantFeatureSetting>()
                .Property(e => e.TenantId)
                .CreateIndex("IX_TenantId_Name", 1);

            modelBuilder.Entity<TenantFeatureSetting>()
                .Property(e => e.Name)
                .CreateIndex("IX_TenantId_Name", 2);

            #endregion

            #region EditionFeatureSetting.IX_EditionId_Name

            modelBuilder.Entity<EditionFeatureSetting>()
                .Property(e => e.EditionId)
                .CreateIndex("IX_EditionId_Name", 1);

            modelBuilder.Entity<EditionFeatureSetting>()
                .Property(e => e.Name)
                .CreateIndex("IX_EditionId_Name", 2);

            #endregion

            #region TTenant.IX_TenancyName

            modelBuilder.Entity<TTenant>()
                .Property(e => e.TenancyName)
                .CreateIndex("IX_TenancyName", 1);

            #endregion

            #region TTenant.Set_ForeignKeys

            modelBuilder.Entity<TTenant>()
                .HasOptional(p => p.DeleterUser)
                .WithMany()
                .HasForeignKey(p => p.DeleterUserId);

            modelBuilder.Entity<TTenant>()
                .HasOptional(p => p.CreatorUser)
                .WithMany()
                .HasForeignKey(p => p.CreatorUserId);

            modelBuilder.Entity<TTenant>()
                .HasOptional(p => p.LastModifierUser)
                .WithMany()
                .HasForeignKey(p => p.LastModifierUserId);

            #endregion

            #region UserAccount.IX_TenantId_UserId

            modelBuilder.Entity<UserAccount>()
                .Property(e => e.TenantId)
                .CreateIndex("IX_TenantId_UserId", 1);

            modelBuilder.Entity<UserAccount>()
                .Property(e => e.UserId)
                .CreateIndex("IX_TenantId_UserId", 2);

            #endregion

            #region UserAccount.IX_TenantId_UserName

            modelBuilder.Entity<UserAccount>()
                .Property(e => e.TenantId)
                .CreateIndex("IX_TenantId_UserName", 1);

            modelBuilder.Entity<UserAccount>()
                .Property(e => e.UserName)
                .CreateIndex("IX_TenantId_UserName", 2);

            #endregion

            #region UserAccount.IX_TenantId_EmailAddress

            modelBuilder.Entity<UserAccount>()
                .Property(e => e.TenantId)
                .CreateIndex("IX_TenantId_EmailAddress", 1);

            modelBuilder.Entity<UserAccount>()
                .Property(e => e.EmailAddress)
                .CreateIndex("IX_TenantId_EmailAddress", 2);

            #endregion

            #region UserAccount.IX_UserName

            modelBuilder.Entity<UserAccount>()
                .Property(e => e.UserName)
                .CreateIndex("IX_UserName", 1);

            #endregion

            #region UserAccount.IX_EmailAddress

            modelBuilder.Entity<UserAccount>()
                .Property(e => e.EmailAddress)
                .CreateIndex("IX_EmailAddress", 1);

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

        }
    }
}
