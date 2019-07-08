using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.MultiTenancy;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    /// <summary>
    /// Base class for role mapping.
    /// </summary>
    public abstract class GeevRoleMap<TRole, TUser> : EntityMap<TRole>
        where TRole : GeevRole<TUser>
        where TUser : GeevUser<TUser>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        protected GeevRoleMap()
            : base("GeevRoles")
        {
            Map(x => x.TenantId);
            Map(x => x.Name);
            Map(x => x.DisplayName);
            Map(x => x.IsStatic);
            Map(x => x.IsDefault);
            
            this.MapFullAudited();

            Polymorphism.Explicit();
        }
    }
}