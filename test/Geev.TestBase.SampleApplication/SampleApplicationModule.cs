using System.Collections.Generic;
using System.Reflection;
using Geev.AutoMapper;
using Geev.Configuration;
using Geev.EntityFramework;
using Geev.EntityFramework.GraphDiff;
using Geev.EntityFramework.GraphDiff.Configuration;
using Geev.EntityFramework.GraphDiff.Mapping;
using Geev.Modules;
using Geev.TestBase.SampleApplication.ContacLists;
using Geev.TestBase.SampleApplication.People;
using AutoMapper;
using RefactorThis.GraphDiff;

namespace Geev.TestBase.SampleApplication
{
    [DependsOn(typeof(GeevEntityFrameworkModule), typeof(GeevAutoMapperModule),
        typeof(GeevEntityFrameworkGraphDiffModule))]
    public class SampleApplicationModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.Features.Providers.Add<SampleFeatureProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.GeevEfGraphDiff().EntityMappings = new List<EntityMapping>
            {
                MappingExpressionBuilder.For<ContactList>(
                    config => config.AssociatedCollection(entity => entity.People)),
                MappingExpressionBuilder.For<Person>(config => config.AssociatedEntity(entity => entity.ContactList))
            };
        }
    }
}
