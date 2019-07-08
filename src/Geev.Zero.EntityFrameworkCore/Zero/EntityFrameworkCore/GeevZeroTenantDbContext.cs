using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Geev.Zero.EntityFrameworkCore
{
    [MultiTenancySide(MultiTenancySides.Tenant)]
    public abstract class GeevZeroTenantDbContext<TRole, TUser,TSelf> : GeevZeroCommonDbContext<TRole, TUser,TSelf>
        where TRole : GeevRole<TUser>
        where TUser : GeevUser<TUser>
        where TSelf: GeevZeroTenantDbContext<TRole, TUser, TSelf>
    {

        /// <summary>
        ///
        /// </summary>
        /// <param name="options"></param>
        protected GeevZeroTenantDbContext(DbContextOptions<TSelf> options)
            :base(options)
        {

        }
    }
}