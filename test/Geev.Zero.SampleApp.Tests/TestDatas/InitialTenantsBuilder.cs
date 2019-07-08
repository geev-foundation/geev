using Geev.Zero.SampleApp.EntityFramework;
using Geev.Zero.SampleApp.MultiTenancy;

namespace Geev.Zero.SampleApp.Tests.TestDatas
{
    public class InitialTenantsBuilder
    {
        private readonly AppDbContext _context;

        public InitialTenantsBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            CreateTenants();
        }

        private void CreateTenants()
        {
            _context.Tenants.Add(new Tenant(Tenant.DefaultTenantName, Tenant.DefaultTenantName));
            _context.SaveChanges();
        }
    }
}