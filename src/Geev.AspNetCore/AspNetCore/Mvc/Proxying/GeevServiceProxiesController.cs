using Geev.AspNetCore.Mvc.Controllers;
using Geev.Auditing;
using Geev.Web.Api.ProxyScripting;
using Geev.Web.Minifier;
using Geev.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geev.AspNetCore.Mvc.Proxying
{
    [DontWrapResult]
    [DisableAuditing]
    public class GeevServiceProxiesController : GeevController
    {
        private readonly IApiProxyScriptManager _proxyScriptManager;
        private readonly IJavaScriptMinifier _javaScriptMinifier;

        public GeevServiceProxiesController(IApiProxyScriptManager proxyScriptManager, 
            IJavaScriptMinifier javaScriptMinifier)
        {
            _proxyScriptManager = proxyScriptManager;
            _javaScriptMinifier = javaScriptMinifier;
        }

        [Produces("application/x-javascript")]
        public ContentResult GetAll(ApiProxyGenerationModel model)
        {
            var script = _proxyScriptManager.GetScript(model.CreateOptions());
            return Content(model.Minify ? _javaScriptMinifier.Minify(script) : script, "application/x-javascript");
        }
    }
}
