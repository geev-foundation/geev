using Geev.Authorization.Roles;

namespace Geev.ZeroCore.SampleApp.Core
{
    public class Role : GeevRole<User>
    {
        public Role()
        {

        }

        public Role(int? tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        {

        }
    }
}