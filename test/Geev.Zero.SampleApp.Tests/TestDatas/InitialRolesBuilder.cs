using System.Linq;
using Geev.Zero.SampleApp.EntityFramework;
using Geev.Zero.SampleApp.MultiTenancy;
using Geev.Zero.SampleApp.Roles;

namespace Geev.Zero.SampleApp.Tests.TestDatas
{
    public class InitialRolesBuilder
    {
        private readonly AppDbContext _context;

        public InitialRolesBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            CreateRoles();
        }

        private void CreateRoles()
        {
            var defaultTenant = _context.Tenants.Single(t => t.TenancyName == Tenant.DefaultTenantName);

            CreateRole(new Role
            {
                TenantId = defaultTenant.Id,
                DisplayName = "TestRole1",
                Name = "test_role_1"
            });

            CreateRole(new Role
            {
                TenantId = defaultTenant.Id,
                DisplayName = "TestRole2",
                Name = "test_role_2"
            });
        }

        private void CreateRole(Role role)
        {
            role.SetNormalizedName();
            _context.Roles.Add(role);
        }
    }
}