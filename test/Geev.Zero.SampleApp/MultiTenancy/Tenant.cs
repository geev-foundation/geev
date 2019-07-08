using Geev.MultiTenancy;
using Geev.Zero.SampleApp.Users;

namespace Geev.Zero.SampleApp.MultiTenancy
{
    public class Tenant : GeevTenant<User>
    {
        protected Tenant()
        {

        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}