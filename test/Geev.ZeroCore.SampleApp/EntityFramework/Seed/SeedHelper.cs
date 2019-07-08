﻿using System;
using System.Transactions;
using Geev.Dependency;
using Geev.Domain.Uow;
using Geev.EntityFrameworkCore.Uow;
using Geev.MultiTenancy;
using Geev.ZeroCore.SampleApp.EntityFramework.Seed.Host;
using Geev.ZeroCore.SampleApp.EntityFramework.Seed.Tenants;
using Microsoft.EntityFrameworkCore;

namespace Geev.ZeroCore.SampleApp.EntityFramework.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<SampleAppDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(SampleAppDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            //Host seed
            new InitialHostDbBuilder(context).Create();

            //Default tenant seed (in host database).
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}
