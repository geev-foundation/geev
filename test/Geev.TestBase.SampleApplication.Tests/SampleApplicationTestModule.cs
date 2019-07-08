using System.Reflection;
using Geev.AutoMapper;
using Geev.Modules;
using Geev.TestBase.SampleApplication.Tests.Uow;

namespace Geev.TestBase.SampleApplication.Tests
{
    [DependsOn(typeof(SampleApplicationModule), typeof(GeevTestBaseModule), typeof(GeevAutoMapperModule))]
    public class SampleApplicationTestModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.GeevAutoMapper().UseStaticMapper = false;

            Configuration.UnitOfWork.ConventionalUowSelectors.Add(type => type == typeof(MyCustomUowClass));
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
