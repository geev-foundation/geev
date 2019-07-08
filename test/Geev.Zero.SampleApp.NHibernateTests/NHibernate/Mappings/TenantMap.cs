using Geev.Zero.NHibernate.EntityMappings;
using Geev.Zero.SampleApp.MultiTenancy;
using Geev.Zero.SampleApp.Users;

namespace Geev.Zero.SampleApp.NHibernate.Mappings
{
    public class TenantMap : GeevTenantMap<Tenant, User>
    {

    }
}
