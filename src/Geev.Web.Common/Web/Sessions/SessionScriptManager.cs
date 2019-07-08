using System.Text;
using Geev.Dependency;
using Geev.Runtime.Session;

namespace Geev.Web.Sessions
{
    public class SessionScriptManager : ISessionScriptManager, ITransientDependency
    {
        public IGeevSession GeevSession { get; set; }

        public SessionScriptManager()
        {
            GeevSession = NullGeevSession.Instance;
        }

        public string GetScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine();

            script.AppendLine("    geev.session = geev.session || {};");
            script.AppendLine("    geev.session.userId = " + (GeevSession.UserId.HasValue ? GeevSession.UserId.Value.ToString() : "null") + ";");
            script.AppendLine("    geev.session.tenantId = " + (GeevSession.TenantId.HasValue ? GeevSession.TenantId.Value.ToString() : "null") + ";");
            script.AppendLine("    geev.session.impersonatorUserId = " + (GeevSession.ImpersonatorUserId.HasValue ? GeevSession.ImpersonatorUserId.Value.ToString() : "null") + ";");
            script.AppendLine("    geev.session.impersonatorTenantId = " + (GeevSession.ImpersonatorTenantId.HasValue ? GeevSession.ImpersonatorTenantId.Value.ToString() : "null") + ";");
            script.AppendLine("    geev.session.multiTenancySide = " + ((int)GeevSession.MultiTenancySide) + ";");

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}