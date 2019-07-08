using System.Net.Http;
using System.Net.Http.Headers;
using Geev.Auditing;
using Geev.Web.Models;
using Geev.Web.Security;
using Geev.Web.Security.AntiForgery;
using Geev.WebApi.Controllers.Dynamic.Formatters;
using Geev.WebApi.Controllers.Dynamic.Scripting.TypeScript;

namespace Geev.WebApi.Controllers.Dynamic.Scripting
{
    [DontWrapResult]
    [DisableAuditing]
    [DisableGeevAntiForgeryTokenValidation]
    public class TypeScriptController : GeevApiController
    {
        readonly TypeScriptDefinitionGenerator _typeScriptDefinitionGenerator;
        readonly TypeScriptServiceGenerator _typeScriptServiceGenerator;
        public TypeScriptController(TypeScriptDefinitionGenerator typeScriptDefinitionGenerator, TypeScriptServiceGenerator typeScriptServiceGenerator)
        {
            _typeScriptDefinitionGenerator = typeScriptDefinitionGenerator;
            _typeScriptServiceGenerator = typeScriptServiceGenerator;
        }
        
        public HttpResponseMessage Get(bool isCompleteService = false)
        {
            if (isCompleteService)
            {
                var script = _typeScriptServiceGenerator.GetScript();
                var response = Request.CreateResponse(System.Net.HttpStatusCode.OK, script, new PlainTextFormatter());
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-javascript");
                return response;
            }
            else
            {
                var script = _typeScriptDefinitionGenerator.GetScript();
                var response = Request.CreateResponse(System.Net.HttpStatusCode.OK, script, new PlainTextFormatter());
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-javascript");
                return response;
            }
        }
    }
}
