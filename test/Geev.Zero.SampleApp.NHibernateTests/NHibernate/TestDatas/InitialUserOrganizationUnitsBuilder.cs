using System.Linq;
using Geev.Authorization.Users;
using Geev.Organizations;
using Geev.Zero.SampleApp.MultiTenancy;
using Geev.Zero.SampleApp.Users;
using NHibernate;
using NHibernate.Linq;

namespace Geev.Zero.SampleApp.NHibernate.TestDatas
{
    public class InitialUserOrganizationUnitsBuilder
    {
        private readonly ISession _session;

        public InitialUserOrganizationUnitsBuilder(ISession session)
        {
            _session = session;
        }

        public void Build()
        {
            AddUsersToOus();
        }

        private void AddUsersToOus()
        {
            var defaultTenant = _session.Query<Tenant>().Single(t => t.Name == Tenant.DefaultTenantName);
            var adminUser = _session.Query<User>().Single(u => u.TenantId == defaultTenant.Id && u.UserName == User.AdminUserName);

            var ou11 = _session.Query<OrganizationUnit>().Single(ou => ou.DisplayName == "OU11");
            var ou21 = _session.Query<OrganizationUnit>().Single(ou => ou.DisplayName == "OU21");

            _session.Save(new UserOrganizationUnit(defaultTenant.Id, adminUser.Id, ou11.Id));
            _session.Save(new UserOrganizationUnit(defaultTenant.Id, adminUser.Id, ou21.Id));
        }
    }
}