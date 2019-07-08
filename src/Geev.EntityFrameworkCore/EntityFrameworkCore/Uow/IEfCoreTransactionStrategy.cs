using Geev.Dependency;
using Geev.Domain.Uow;
using Microsoft.EntityFrameworkCore;

namespace Geev.EntityFrameworkCore.Uow
{
  public interface IEfCoreTransactionStrategy
  {
    void InitOptions(UnitOfWorkOptions options);

    DbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver)
        where TDbContext : DbContext;

    void Commit();

    void Dispose(IIocResolver iocResolver);
  }
}
