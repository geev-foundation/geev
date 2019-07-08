using Geev.Application.Editions;
using Geev.Application.Features;
using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.BackgroundJobs;
using Geev.MultiTenancy;
using Geev.Notifications;
using Microsoft.EntityFrameworkCore;

namespace Geev.Zero.EntityFrameworkCore
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
        /// 
        /// </summary>
        /// <param name="options"></param>
        protected GeevZeroDbContext(DbContextOptions<TSelf> options)
            : base(options)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region BackgroundJobInfo.IX_IsAbandoned_NextTryTime


            modelBuilder.Entity<BackgroundJobInfo>()
                .HasIndex(j => new { j.IsAbandoned, j.NextTryTime });

            #endregion

            #region NotificationSubscriptionInfo.IX_NotificationName_EntityTypeName_EntityId_UserId

            modelBuilder.Entity<NotificationSubscriptionInfo>()
                .HasIndex(ns => new
                {
                    ns.NotificationName,
                    ns.EntityTypeName,
                    ns.EntityId,
                    ns.UserId
                });

            #endregion

            #region UserNotificationInfo.IX_UserId_State_CreationTime

            modelBuilder.Entity<UserNotificationInfo>()
                .HasIndex(un => new { un.UserId, un.State, un.CreationTime });
            #endregion

            #region UserLoginAttempt.IX_TenancyName_UserNameOrEmailAddress_Result

            modelBuilder.Entity<UserLoginAttempt>()
                .HasIndex(ula => new
                {
                    ula.TenancyName,
                    ula.UserNameOrEmailAddress,
                    ula.Result
                });

            #endregion

            #region UserLoginAttempt.IX_UserId_TenantId

            modelBuilder.Entity<UserLoginAttempt>()
                .HasIndex(ula => new { ula.UserId, ula.TenantId });

            #endregion
        }
        
    }
}
