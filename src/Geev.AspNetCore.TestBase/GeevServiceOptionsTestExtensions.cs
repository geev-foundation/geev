using Geev.Dependency;
using Geev.Runtime.Session;
using Geev.TestBase.Runtime.Session;

namespace Geev.AspNetCore.TestBase
{
    public static class GeevServiceOptionsTestExtensions
    {
        public static void SetupTest(this GeevBootstrapperOptions options)
        {
            options.IocManager = new IocManager();
            options.IocManager.RegisterIfNot<IGeevSession, TestGeevSession>();
        }
    }
}