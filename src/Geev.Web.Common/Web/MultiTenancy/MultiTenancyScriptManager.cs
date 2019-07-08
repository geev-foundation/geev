using System;
using System.Globalization;
using System.Text;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Extensions;
using Geev.MultiTenancy;

namespace Geev.Web.MultiTenancy
{
    public class MultiTenancyScriptManager : IMultiTenancyScriptManager, ITransientDependency
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        public MultiTenancyScriptManager(IMultiTenancyConfig multiTenancyConfig)
        {
            _multiTenancyConfig = multiTenancyConfig;
        }

        public string GetScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(geev){");
            script.AppendLine();

            script.AppendLine("    geev.multiTenancy = geev.multiTenancy || {};");
            script.AppendLine("    geev.multiTenancy.isEnabled = " + _multiTenancyConfig.IsEnabled.ToString().ToLowerInvariant() + ";");

            script.AppendLine();
            script.Append("})(geev);");

            return script.ToString();
        }
    }
}