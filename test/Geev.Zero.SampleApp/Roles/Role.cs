using Geev.Authorization.Roles;
using Geev.Zero.SampleApp.Users;

namespace Geev.Zero.SampleApp.Roles
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