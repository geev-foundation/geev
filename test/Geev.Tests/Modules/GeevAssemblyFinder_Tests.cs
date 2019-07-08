using System.Linq;
using System.Reflection;
using Geev.Modules;
using Geev.Reflection;
using Geev.Reflection.Extensions;
using Shouldly;
using Xunit;

namespace Geev.Tests.Modules
{
    public class GeevAssemblyFinder_Tests: TestBaseWithLocalIocManager
    {
        [Fact]
        public void Should_Get_Module_And_Additional_Assemblies()
        {
            //Arrange
            var bootstrapper = GeevBootstrapper.Create<MyStartupModule>(options =>
            {
                options.IocManager = LocalIocManager;
            });

            bootstrapper.Initialize();

            //Act
            var assemblies = bootstrapper.IocManager.Resolve<GeevAssemblyFinder>().GetAllAssemblies();

            //Assert
            assemblies.Count.ShouldBe(3);

            assemblies.Any(a => a == typeof(MyStartupModule).GetAssembly()).ShouldBeTrue();
            assemblies.Any(a => a == typeof(GeevKernelModule).GetAssembly()).ShouldBeTrue();
            assemblies.Any(a => a == typeof(FactAttribute).GetAssembly()).ShouldBeTrue();
        }

        public class MyStartupModule : GeevModule
        {
            public override Assembly[] GetAdditionalAssemblies()
            {
                return new[] {typeof(FactAttribute).GetAssembly()};
            }
        }
    }
}