using System.Text;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Json;

namespace Geev.Web.Configuration
{
    public class CustomConfigScriptManager : ICustomConfigScriptManager, ITransientDependency
    {
        private readonly IGeevStartupConfiguration _geevStartupConfiguration;

        public CustomConfigScriptManager(IGeevStartupConfiguration geevStartupConfiguration)
        {
            _geevStartupConfiguration = geevStartupConfiguration;
        }

        public string GetScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(geev){");
            script.AppendLine();

            script.AppendLine("    geev.custom = " + _geevStartupConfiguration.GetCustomConfig().ToJsonString());

            script.AppendLine();
            script.Append("})(geev);");

            return script.ToString();
        }
    }
}