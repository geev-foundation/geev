using System;
using System.Collections.Generic;
using Geev.Authorization;
using Geev.Configuration;

namespace Geev.Web.Common.Tests
{
    public class GeevWebCommonTestModuleSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(
                    "GeevWebCommonTestModule.Test.Setting1",
                    "TestValue1",
                    scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User,
                    clientVisibilityProvider:new RequiresAuthenticationSettingClientVisibilityProvider()),

                new SettingDefinition(
                    "GeevWebCommonTestModule.Test.Setting2",
                    "TestValue2",
                    scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User,
                    clientVisibilityProvider:new RequiresPermissionSettingClientVisibilityProvider(new SimplePermissionDependency("Permission1"))),

                new SettingDefinition(
                    "GeevWebCommonTestModule.Test.Setting3",
                    "Test > Value3",
                    scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User,
                    clientVisibilityProvider:new RequiresPermissionSettingClientVisibilityProvider(new SimplePermissionDependency("Permission1")))
            };
        }
    }
}