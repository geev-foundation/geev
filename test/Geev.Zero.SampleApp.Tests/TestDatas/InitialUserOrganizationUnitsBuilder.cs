using System.Linq;
using Geev.Authorization.Users;
using Geev.Zero.SampleApp.EntityFramework;
using Geev.Zero.SampleApp.MultiTenancy;
using Geev.Zero.SampleApp.Users;

namespace Geev.Zero.SampleApp.Tests.TestDatas
{
    public class InitialUserOrganizationUnitsBuilder
    {
        private readonly AppDbContext _context;

        public InitialUserOrganizationUnitsBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            AddUsersToOus();
        }

        private void AddUsersToOus()
        {
            var defaultTenant = _context.Tenants.Single(t => t.TenancyName == Tenant.DefaultTenantName);
            var adminUser = _context.Users.Single(u => u.TenantId == defaultTenant.Id && u.UserName == User.AdminUserName);

            var ou11 = _context.OrganizationUnits.Single(ou => ou.DisplayName == "OU11");
            var ou21 = _context.OrganizationUnits.Single(ou => ou.DisplayName == "OU21");

            _context.UserOrganizationUnits.Add(new UserOrganizationUnit(defaultTenant.Id, adminUser.Id, ou11.Id));
            _context.UserOrganizationUnits.Add(new UserOrganizationUnit(defaultTenant.Id, adminUser.Id, ou21.Id));
        }
    }
}