using Geev.Authorization.Users;
using Geev.MultiTenancy;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    /// <summary>
    /// Base class to map classes derived from <see cref="GeevTenant{TTenant,TUser}"/>
    /// </summary>
    /// <typeparam name="TTenant">Tenant type</typeparam>
    /// <typeparam name="TUser">User type</typeparam>
    public abstract class GeevTenantMap<TTenant, TUser> : EntityMap<TTenant>
        where TTenant : GeevTenant<TUser>
        where TUser : GeevUser<TUser>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        protected GeevTenantMap()
            : base("GeevTenants")
        {
            References(x => x.Edition).Column("EditionId").Nullable();

            Map(x => x.TenancyName);
            Map(x => x.Name);
            Map(x => x.IsActive);

            this.MapFullAudited();

            Polymorphism.Explicit();
        }
    }
}