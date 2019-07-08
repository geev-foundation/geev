using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.MultiTenancy;

namespace Geev.Zero.EntityFramework
{
    [MultiTenancySide(MultiTenancySides.Tenant)]
    public abstract class GeevZeroTenantDbContext<TRole, TUser,TSelf> : GeevZeroCommonDbContext<TRole, TUser,TSelf>
        where TRole : GeevRole<TUser>
        where TUser : GeevUser<TUser>
        where TSelf: GeevZeroTenantDbContext<TRole, TUser, TSelf>
    {

        /// <summary>
        /// Default constructor.
        /// Do not directly instantiate this class. Instead, use dependency injection!
        /// </summary>
        protected GeevZeroTenantDbContext()
        {

        }

        /// <summary>
        /// Constructor with connection string parameter.
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string or a name in connection strings in configuration file</param>
        protected GeevZeroTenantDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        protected GeevZeroTenantDbContext(DbCompiledModel model)
            : base(model)
        {

        }

        /// <summary>
        /// This constructor can be used for unit tests.
        /// </summary>
        protected GeevZeroTenantDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {

        }

        protected GeevZeroTenantDbContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
        }

        protected GeevZeroTenantDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
        }

        protected GeevZeroTenantDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }
    }
}