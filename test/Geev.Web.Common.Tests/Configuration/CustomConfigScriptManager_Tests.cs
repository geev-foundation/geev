using System.Collections.Generic;
using Geev.Configuration;
using Geev.Configuration.Startup;
using Geev.TestBase;
using Geev.Web.Configuration;
using Shouldly;
using Xunit;

namespace Geev.Web.Common.Tests.Configuration
{
    public class CustomConfigScriptManager_Tests : GeevIntegratedTestBase<GeevWebCommonTestModule>
    {
        private readonly ICustomConfigScriptManager _customConfigScriptManager;
        private readonly IGeevStartupConfiguration _geevStartupConfiguration;

        public CustomConfigScriptManager_Tests()
        {
            _geevStartupConfiguration = Resolve<IGeevStartupConfiguration>();
            _customConfigScriptManager = Resolve<ICustomConfigScriptManager>();
        }

        [Fact]
        public void CustomConfigScriptManager_Should_Build_Custom_Configuration()
        {
            _geevStartupConfiguration.CustomConfigProviders.Add(new TestCustomConfigProvider());

            var script = _customConfigScriptManager.GetScript();
            script.ShouldNotBeNullOrEmpty();
            script.ShouldContain("EntityHistory");
        }

        [Fact]
        public void CustomConfigScriptManager_Should_Build_Empty_Custom_Configuration_When_CustomConfigProviders_Empty()
        {
            _geevStartupConfiguration.CustomConfigProviders.Clear();

            var script = _customConfigScriptManager.GetScript();
            script.ShouldNotBeNullOrEmpty();
        }
    }

    public class TestCustomConfigProvider : ICustomConfigProvider
    {
        public Dictionary<string, object> GetConfig(CustomConfigProviderContext customConfigProviderContext)
        {
            return new Dictionary<string, object>
            {
                {
                    "EntityHistory",
                    new {
                        IsEnabled = true
                    }
                }
            };
        }
    }
}
