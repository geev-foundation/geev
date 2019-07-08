using System;

namespace Geev.EntityFramework
{
    public class EfBasedSecondaryOrmRegistrar : SecondaryOrmRegistrarBase
    {
        public EfBasedSecondaryOrmRegistrar(Type dbContextType, IDbContextEntityFinder dbContextEntityFinder)
            : base(dbContextType, dbContextEntityFinder)
        {
        }

        public override string OrmContextKey => GeevConsts.Orms.EntityFramework;
    }
}
