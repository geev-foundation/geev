using System.Text;
using System.Threading.Tasks;
using Geev.Configuration;
using Geev.Dependency;
using Geev.Runtime.Session;
using Geev.Web.Http;

namespace Geev.Web.Settings
{
    /// <summary>
    /// This class is used to build setting script.
    /// </summary>
    public class SettingScriptManager : ISettingScriptManager, ISingletonDependency
    {
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly ISettingManager _settingManager;
        private readonly IGeevSession _geevSession;
        private readonly IIocResolver _iocResolver;

        public SettingScriptManager(
            ISettingDefinitionManager settingDefinitionManager,
            ISettingManager settingManager,
            IGeevSession geevSession,
            IIocResolver iocResolver)
        {
            _settingDefinitionManager = settingDefinitionManager;
            _settingManager = settingManager;
            _geevSession = geevSession;
            _iocResolver = iocResolver;
        }

        public async Task<string> GetScriptAsync()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine("    geev.setting = geev.setting || {};");
            script.AppendLine("    geev.setting.values = {");

            var settingDefinitions = _settingDefinitionManager
                .GetAllSettingDefinitions();

            var added = 0;

            using (var scope = _iocResolver.CreateScope())
            {
                foreach (var settingDefinition in settingDefinitions)
                {
                    if (!await settingDefinition.ClientVisibilityProvider.CheckVisible(scope))
                    {
                        continue;
                    }

                    if (added > 0)
                    {
                        script.AppendLine(",");
                    }
                    else
                    {
                        script.AppendLine();
                    }

                    var settingValue = await _settingManager.GetSettingValueAsync(settingDefinition.Name);

                    script.Append("        '" +
                                  HttpEncode.JavaScriptStringEncode(settingDefinition.Name) + "': " +
                                  (settingValue == null ? "null" : "'" + HttpEncode.JavaScriptStringEncode(settingValue) + "'"));

                    ++added;
                }
            }

            script.AppendLine();
            script.AppendLine("    };");

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}