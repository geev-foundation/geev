using System.Text;
using Geev.Dependency;
using Geev.Web.Security.AntiForgery;

namespace Geev.Web.Security
{
    internal class SecurityScriptManager : ISecurityScriptManager, ITransientDependency
    {
        private readonly IGeevAntiForgeryConfiguration _geevAntiForgeryConfiguration;

        public SecurityScriptManager(IGeevAntiForgeryConfiguration geevAntiForgeryConfiguration)
        {
            _geevAntiForgeryConfiguration = geevAntiForgeryConfiguration;
        }

        public string GetScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine("    geev.security.antiForgery.tokenCookieName = '" + _geevAntiForgeryConfiguration.TokenCookieName + "';");
            script.AppendLine("    geev.security.antiForgery.tokenHeaderName = '" + _geevAntiForgeryConfiguration.TokenHeaderName + "';");
            script.Append("})();");

            return script.ToString();
        }
    }
}
