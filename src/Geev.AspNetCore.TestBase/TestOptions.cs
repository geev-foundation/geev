using Geev.Collections;
using Geev.Modules;

namespace Geev.AspNetCore.TestBase
{
    public class TestOptions
    {
        public ITypeList<GeevModule> Modules { get; private set; }

        public TestOptions()
        {
            Modules = new TypeList<GeevModule>();
        }
    }
}