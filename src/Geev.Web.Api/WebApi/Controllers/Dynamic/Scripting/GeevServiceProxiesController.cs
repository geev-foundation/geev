using System.Net.Http;
using System.Net.Http.Headers;
using Geev.Auditing;
using Geev.Web.Minifier;
using Geev.Web.Models;
using Geev.Web.Security.AntiForgery;
using Geev.WebApi.Controllers.Dynamic.Formatters;

namespace Geev.WebApi.Controllers.Dynamic.Scripting
{
    /// <summary>
    /// This class is used to create proxies to call dynamic api methods from JavaScript clients.
    /// </summary>
    [DontWrapResult]
    [DisableAuditing]
    [DisableGeevAntiForgeryTokenValidation]
    public class GeevServiceProxiesController : GeevApiController
    {
        private readonly ScriptProxyManager _scriptProxyManager;
        private readonly IJavaScriptMinifier _javaScriptMinifier;

        public GeevServiceProxiesController(ScriptProxyManager scriptProxyManager, 
            IJavaScriptMinifier javaScriptMinifier)
        {
            _scriptProxyManager = scriptProxyManager;
            _javaScriptMinifier = javaScriptMinifier;
        }

        /// <summary>
        /// Gets JavaScript proxy for given service name.
        /// </summary>
        /// <param name="name">Name of the service</param>
        /// <param name="type">Script type</param>
        /// <param name="minify">Minify the JavaScript Code</param>
        public HttpResponseMessage Get(string name, ProxyScriptType type = ProxyScriptType.JQuery, bool minify = false)
        {
            var script = _scriptProxyManager.GetScript(name, type);
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK,
                minify ? _javaScriptMinifier.Minify(script) : script, new PlainTextFormatter());
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-javascript");
            return response;
        }

        /// <summary>
        /// Gets JavaScript proxy for all services.
        /// </summary>
        /// <param name="type">Script type</param>
        /// <param name="minify"></param>
        public HttpResponseMessage GetAll(ProxyScriptType type = ProxyScriptType.JQuery, bool minify = false)
        {
            var script = _scriptProxyManager.GetAllScript(type);
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK,
                minify ? _javaScriptMinifier.Minify(script) : script, new PlainTextFormatter());
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-javascript");
            return response;
        }
    }
}