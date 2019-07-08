using System;
using System.Transactions;
using Geev.Data;
using Geev.Dependency;
using Geev.Domain.Uow;
using Geev.EntityFrameworkCore;
using Geev.Extensions;
using Geev.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Geev.Zero.EntityFrameworkCore
{
    public abstract class GeevZeroDbMigrator<TDbContext> : IGeevZeroDbMigrator, ITransientDependency
        where TDbContext : DbContext
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDbPerTenantConnectionStringResolver _connectionStringResolver;
        private readonly IDbContextResolver _dbContextResolver;

        protected GeevZeroDbMigrator(
            IUnitOfWorkManager unitOfWorkManager,
            IDbPerTenantConnectionStringResolver connectionStringResolver,
            IDbContextResolver dbContextResolver)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
            _dbContextResolver = dbContextResolver;
        }
        
        public virtual void CreateOrMigrateForHost()
        {
            CreateOrMigrateForHost(null);
        }

        public virtual void CreateOrMigrateForHost(Action<TDbContext> seedAction)
        {
            CreateOrMigrate(null, seedAction);
        }

        public virtual void CreateOrMigrateForTenant(GeevTenantBase tenant)
        {
            CreateOrMigrateForTenant(tenant, null);
        }

        public virtual void CreateOrMigrateForTenant(GeevTenantBase tenant, Action<TDbContext> seedAction)
        {
            if (tenant.ConnectionString.IsNullOrEmpty())
            {
                return;
            }

            CreateOrMigrate(tenant, seedAction);
        }

        protected virtual void CreateOrMigrate(GeevTenantBase tenant, Action<TDbContext> seedAction)
        {
            var args = new DbPerTenantConnectionStringResolveArgs(
                tenant == null ? (int?) null : (int?) tenant.Id,
                tenant == null ? MultiTenancySides.Host : MultiTenancySides.Tenant
            );

            args["DbContextType"] = typeof(TDbContext);
            args["DbContextConcreteType"] = typeof(TDbContext);

            var nameOrConnectionString = ConnectionStringHelper.GetConnectionString(
                _connectionStringResolver.GetNameOrConnectionString(args)
            );

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                using (var dbContext = _dbContextResolver.Resolve<TDbContext>(nameOrConnectionString, null))
                {
                    dbContext.Database.Migrate();
                    seedAction?.Invoke(dbContext);
                    _unitOfWorkManager.Current.SaveChanges();
                    uow.Complete();
                }
            }
        }
    }
}
