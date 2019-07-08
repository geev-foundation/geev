using System.Linq;
using Geev.Modules;
using Geev.PlugIns;
using Shouldly;
using Xunit;

namespace Geev.Tests.Modules
{
    public class PlugInModuleLoading_Tests : TestBaseWithLocalIocManager
    {
        [Fact]
        public void Should_Load_All_Modules()
        {
            //Arrange
            var bootstrapper = GeevBootstrapper.Create<MyStartupModule>(options =>
            {
                options.IocManager = LocalIocManager;
            });

            bootstrapper.PlugInSources.AddTypeList(typeof(MyPlugInModule));

            bootstrapper.Initialize();

            //Act
            var modules = bootstrapper.IocManager.Resolve<IGeevModuleManager>().Modules;

            //Assert
            modules.Count.ShouldBe(6);

            modules.Any(m => m.Type == typeof(GeevKernelModule)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyStartupModule)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyModule1)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyModule2)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyPlugInModule)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyPlugInDependedModule)).ShouldBeTrue();

            modules.Any(m => m.Type == typeof(MyNotDependedModule)).ShouldBeFalse();
        }

        [DependsOn(typeof(MyModule1), typeof(MyModule2))]
        public class MyStartupModule: GeevModule
        {

        }

        public class MyModule1 : GeevModule
        {
            
        }

        public class MyModule2 : GeevModule
        {

        }
        
        public class MyNotDependedModule : GeevModule
        {

        }

        [DependsOn(typeof(MyPlugInDependedModule))]
        public class MyPlugInModule : GeevModule
        {
            
        }

        public class MyPlugInDependedModule : GeevModule
        {
            
        }
    }
}
