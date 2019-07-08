using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Geev.Modules;

namespace Geev.Reflection
{
    public class GeevAssemblyFinder : IAssemblyFinder
    {
        private readonly IGeevModuleManager _moduleManager;

        public GeevAssemblyFinder(IGeevModuleManager moduleManager)
        {
            _moduleManager = moduleManager;
        }

        public List<Assembly> GetAllAssemblies()
        {
            var assemblies = new List<Assembly>();

            foreach (var module in _moduleManager.Modules)
            {
                assemblies.Add(module.Assembly);
                assemblies.AddRange(module.Instance.GetAdditionalAssemblies());
            }

            return assemblies.Distinct().ToList();
        }
    }
}