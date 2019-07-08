using System;
using System.Threading.Tasks;
using Geev.Dependency;
using Geev.Domain.Uow;
using Geev.EntityFrameworkCore;
using Geev.TestBase;
using Geev.Zero.SampleApp.EntityFrameworkCore.TestDataBuilders.HostDatas;

namespace Geev.Zero.SampleApp.EntityFrameworkCore.Tests
{
    public class SimpleTaskAppTestBase : GeevIntegratedTestBase<SampleAppTestModule>
    {
        public SimpleTaskAppTestBase()
        {
            UsingDbContext(context => new HostDataBuilder(context).Build());
        }

        protected virtual void UsingDbContext(Action<AppDbContext> action)
        {
            using (var uow = Resolve<IUnitOfWorkManager>().Begin())
            {
                using (var contextProvider = LocalIocManager.ResolveAsDisposable<IDbContextProvider<AppDbContext>>())
                {
                    var dbContext = contextProvider.Object.GetDbContext();

                    action(dbContext);
                    dbContext.SaveChanges(true);
                }

                uow.Complete();
            }
        }

        protected virtual T UsingDbContext<T>(Func<AppDbContext, T> func)
        {
            T result;

            using (var uow = Resolve<IUnitOfWorkManager>().Begin())
            {
                using (var contextProvider = LocalIocManager.ResolveAsDisposable<IDbContextProvider<AppDbContext>>())
                {
                    var dbContext = contextProvider.Object.GetDbContext();

                    result = func(dbContext);
                    dbContext.SaveChanges(true);
                }

                uow.Complete();
            }

            return result;
        }

        protected virtual async Task UsingDbContextAsync(Func<AppDbContext, Task> action)
        {
            using (var uow = Resolve<IUnitOfWorkManager>().Begin())
            {
                using (var contextProvider = LocalIocManager.ResolveAsDisposable<IDbContextProvider<AppDbContext>>())
                {
                    var dbContext = contextProvider.Object.GetDbContext();                    

                    await action(dbContext);
                    await dbContext.SaveChangesAsync(true);
                }

                await uow.CompleteAsync();
            }
        }

        protected virtual async Task<T> UsingDbContextAsync<T>(Func<AppDbContext, Task<T>> func)
        {
            T result;

            using (var uow = Resolve<IUnitOfWorkManager>().Begin())
            {
                using (var contextProvider = LocalIocManager.ResolveAsDisposable<IDbContextProvider<AppDbContext>>())
                {
                    var dbContext = contextProvider.Object.GetDbContext();

                    result = await func(dbContext);
                    await dbContext.SaveChangesAsync(true);
                }

                await uow.CompleteAsync();
            }

            return result;
        }
    }
}