using System.Threading.Tasks;
using Geev.AspNetCore.Mvc.Proxying;
using Geev.Web.Api.ProxyScripting.Generators.JQuery;
using Shouldly;
using Xunit;

namespace Geev.AspNetCore.Tests
{
    public class ProxyScripting_Tests : AppTestBase
    {
        [Fact]
        public async Task jQuery_Scripting_Simple_Test()
        {
            // Act
            var response = await GetResponseAsStringAsync(
                GetUrl<GeevServiceProxiesController>(
                    nameof(GeevServiceProxiesController.GetAll),
                    new { type = JQueryProxyScriptGenerator.Name }
                )
            );

            response.ShouldNotBeNullOrEmpty();
        }
    }
}
