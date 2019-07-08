﻿using Geev.Configuration.Startup;
using Geev.Web.MultiTenancy;
using Shouldly;
using Xunit;

namespace Geev.Web.Tests.MultiTenancy
{
    public class MultiTenancyScriptManager_Tests
    {
        [Fact]
        public void Should_Get_Script()
        {
            var scriptManager = new MultiTenancyScriptManager(new MultiTenancyConfig {IsEnabled = true});
            var script = scriptManager.GetScript();
            script.ShouldNotBe(null);
        }
    }
}
