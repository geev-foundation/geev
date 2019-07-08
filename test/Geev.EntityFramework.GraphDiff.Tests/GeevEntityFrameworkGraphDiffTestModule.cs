using System.Collections.Generic;
using System.Reflection;
using Geev.EntityFramework.GraphDiff;
using Geev.EntityFramework.GraphDiff.Configuration;
using Geev.EntityFramework.GraphDiff.Mapping;
using Geev.EntityFramework.GraphDIff.Tests.Entities;
using Geev.Modules;
using Geev.TestBase;
using RefactorThis.GraphDiff;

namespace Geev.EntityFramework.GraphDIff.Tests
{
    [DependsOn(typeof(GeevEntityFrameworkGraphDiffModule), typeof(GeevTestBaseModule))]
    public class GeevEntityFrameworkGraphDiffTestModule : GeevModule
    {
        public override void Initialize()
        {
            base.Initialize();

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.GeevEfGraphDiff().EntityMappings = new List<EntityMapping>
            {
                MappingExpressionBuilder.For<MyMainEntity>(config => config.AssociatedCollection(entity => entity.MyDependentEntities)),
                MappingExpressionBuilder.For<MyDependentEntity>(config => config.AssociatedEntity(entity => entity.MyMainEntity))
            };
        }
    }
}