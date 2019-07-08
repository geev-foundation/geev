namespace Geev.Configuration.Startup
{
    internal class ModuleConfigurations : IModuleConfigurations
    {
        public IGeevStartupConfiguration GeevConfiguration { get; private set; }

        public ModuleConfigurations(IGeevStartupConfiguration geevConfiguration)
        {
            GeevConfiguration = geevConfiguration;
        }
    }
}