using Geev.MultiTenancy;

namespace Geev.ZeroCore.SampleApp.Core
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