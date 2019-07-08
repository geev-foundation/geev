using Geev.Application.Editions;
using Geev.Application.Features;
using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.BackgroundJobs;
using Geev.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Geev.Zero.EntityFrameworkCore
{
    [MultiTenancySide(MultiTenancySides.Host)]
    public abstract class GeevZeroHostDbContext<TTenant, TRole, TUser, TSelf> : GeevZeroCommonDbContext<TRole, TUser, TSelf>
        where TTenant : GeevTenant<TUser>
        where TRole : GeevRole<TUser>
        where TUser : GeevUser<TUser>
        where TSelf : GeevZeroHostDbContext<TTenant, TRole, TUser, TSelf>
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
        protected GeevZeroHostDbContext(DbContextOptions<TSelf> options)
            :base(options)
        {

        }
    }
}