using System.Linq;

using Geev.Modules;
using Geev.PlugIns;

using Shouldly;

using Xunit;

namespace Geev.Tests.Modules
{
    public class StartupModuleToBeLast_Tests : TestBaseWithLocalIocManager
    {
        [Fact]
        public void StartupModule_ShouldBe_LastModule()
        {
            //Arrange
            var bootstrapper = GeevBootstrapper.Create<MyStartupModule>(options =>
            {
                options.IocManager = LocalIocManager;
            });
            bootstrapper.Initialize();

            //Act
            var modules = bootstrapper.IocManager.Resolve<IGeevModuleManager>().Modules;

            //Assert
            modules.Count.ShouldBe(4);

            modules.Any(m => m.Type == typeof(GeevKernelModule)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyStartupModule)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyModule1)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyModule2)).ShouldBeTrue();

            var startupModule = modules.Last();

            startupModule.Type.ShouldBe(typeof(MyStartupModule));
        }

        [Fact]
        public void PluginModule_ShouldNotBeLast()
        {
            var bootstrapper = GeevBootstrapper.Create<MyStartupModule>(options =>
            {
                options.IocManager = LocalIocManager;
            });

            bootstrapper.PlugInSources.AddTypeList(typeof(MyPlugInModule));

            bootstrapper.Initialize();

            var modules = bootstrapper.IocManager.Resolve<IGeevModuleManager>().Modules;

            //Assert
            modules.Count.ShouldBe(6);

            modules.Any(m => m.Type == typeof(GeevKernelModule)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyStartupModule)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyModule1)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyModule2)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyPlugInModule)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyPlugInDependedModule)).ShouldBeTrue();

            modules.Last().Type.ShouldBe(typeof(MyStartupModule));
        }

        [DependsOn(typeof(MyModule1), typeof(MyModule2))]
        public class MyStartupModule : GeevModule {}

        public class MyModule1 : GeevModule {}

        public class MyModule2 : GeevModule {}

        [DependsOn(typeof(MyPlugInDependedModule))]
        public class MyPlugInModule : GeevModule {}

        public class MyPlugInDependedModule : GeevModule {}
    }
}
