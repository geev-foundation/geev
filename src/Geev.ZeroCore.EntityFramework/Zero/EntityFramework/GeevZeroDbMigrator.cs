﻿using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Transactions;
using Geev.Data;
using Geev.Dependency;
using Geev.Domain.Uow;
using Geev.EntityFramework;
using Geev.Extensions;
using Geev.MultiTenancy;

namespace Geev.Zero.EntityFramework
{
    public abstract class GeevZeroDbMigrator<TDbContext, TConfiguration> : IGeevZeroDbMigrator, ITransientDependency
        where TDbContext : DbContext
        where TConfiguration : DbMigrationsConfiguration<TDbContext>, IMultiTenantSeed, new()
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDbPerTenantConnectionStringResolver _connectionStringResolver;
        private readonly IIocResolver _iocResolver;

        protected GeevZeroDbMigrator(
            IUnitOfWorkManager unitOfWorkManager,
            IDbPerTenantConnectionStringResolver connectionStringResolver,
            IIocResolver iocResolver)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
            _iocResolver = iocResolver;
        }

        public virtual void CreateOrMigrateForHost()
        {
            CreateOrMigrate(null);
        }

        public virtual void CreateOrMigrateForTenant(GeevTenantBase tenant)
        {
            if (tenant.ConnectionString.IsNullOrEmpty())
            {
                return;
            }

            CreateOrMigrate(tenant);
        }

        protected virtual void CreateOrMigrate(GeevTenantBase tenant)
        {
            var args = new DbPerTenantConnectionStringResolveArgs(
                tenant == null ? (int?)null : (int?)tenant.Id,
                tenant == null ? MultiTenancySides.Host : MultiTenancySides.Tenant
                );

            args["DbContextType"] = typeof(TDbContext);
            args["DbContextConcreteType"] = typeof(TDbContext);

            var nameOrConnectionString = ConnectionStringHelper.GetConnectionString(
                _connectionStringResolver.GetNameOrConnectionString(args)
            );

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                using (var dbContext = _iocResolver.ResolveAsDisposable<TDbContext>(new { nameOrConnectionString = nameOrConnectionString }))
                {
                    var dbInitializer = new MigrateDatabaseToLatestVersion<TDbContext, TConfiguration>(
                        true,
                        new TConfiguration
                        {
                            Tenant = tenant
                        });

                    dbInitializer.InitializeDatabase(dbContext.Object);

                    _unitOfWorkManager.Current.SaveChanges();
                    uow.Complete();
                }
            }
        }
    }
}
