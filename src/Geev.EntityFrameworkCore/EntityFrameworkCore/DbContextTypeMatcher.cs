using Geev.Domain.Uow;
using Geev.EntityFramework;

namespace Geev.EntityFrameworkCore
{
    public class DbContextTypeMatcher : DbContextTypeMatcher<GeevDbContext>
    {
        public DbContextTypeMatcher(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
            : base(currentUnitOfWorkProvider)
        {
        }
    }
}