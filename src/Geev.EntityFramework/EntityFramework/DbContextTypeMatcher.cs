using Geev.Domain.Uow;

namespace Geev.EntityFramework
{
    public class DbContextTypeMatcher : DbContextTypeMatcher<GeevDbContext>
    {
        public DbContextTypeMatcher(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider) 
            : base(currentUnitOfWorkProvider)
        {
        }
    }
}