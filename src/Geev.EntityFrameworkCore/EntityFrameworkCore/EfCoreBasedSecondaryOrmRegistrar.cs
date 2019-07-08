using System;

using Geev.EntityFramework;

namespace Geev.EntityFrameworkCore
{
    public class EfCoreBasedSecondaryOrmRegistrar : SecondaryOrmRegistrarBase
    {
        public EfCoreBasedSecondaryOrmRegistrar(Type dbContextType, IDbContextEntityFinder dbContextEntityFinder)
            : base(dbContextType, dbContextEntityFinder)
        {
        }

        public override string OrmContextKey { get; } = GeevConsts.Orms.EntityFrameworkCore;
    }
}
