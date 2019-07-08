using System.Collections.Generic;
using System.Threading.Tasks;
using Geev.Application.Features;
using Geev.Application.Navigation;
using Geev.Authorization;
using Geev.Configuration;
using Geev.Configuration.Startup;
using Geev.Localization;
using Geev.Runtime.Session;
using Geev.Timing;
using Geev.Timing.Timezone;
using Geev.Web.Models.GeevUserConfiguration;
using Geev.Web.Security.AntiForgery;
using System.Linq;
using Geev.Dependency;
using Geev.Extensions;
using System.Globalization;

namespace Geev.Web.Configuration
{
    public class GeevUserConfigurationBuilder : ITransientDependency
    {
        private readonly IGeevStartupConfiguration _startupConfiguration;

        protected IMultiTenancyConfig MultiTenancyConfig { get; }
        protected ILanguageManager LanguageManager { get; }
        protected ILocalizationManager LocalizationManager { get; }
        protected IFeatureManager FeatureManager { get; }
        protected IFeatureChecker FeatureChecker { get; }
        protected IPermissionManager PermissionManager { get; }
        protected IUserNavigationManager UserNavigationManager { get; }
        protected ISettingDefinitionManager SettingDefinitionManager { get; }
        protected ISettingManager SettingManager { get; }
        protected IGeevAntiForgeryConfiguration GeevAntiForgeryConfiguration { get; }
        protected IGeevSession GeevSession { get; }
        protected IPermissionChecker PermissionChecker { get; }
        protected Dictionary<string, object> CustomDataConfig { get; }

        private readonly IIocResolver _iocResolver;

        public GeevUserConfigurationBuilder(
            IMultiTenancyConfig multiTenancyConfig,
            ILanguageManager languageManager,
            ILocalizationManager localizationManager,
            IFeatureManager featureManager,
            IFeatureChecker featureChecker,
            IPermissionManager permissionManager,
            IUserNavigationManager userNavigationManager,
            ISettingDefinitionManager settingDefinitionManager,
            ISettingManager settingManager,
            IGeevAntiForgeryConfiguration geevAntiForgeryConfiguration,
            IGeevSession geevSession,
            IPermissionChecker permissionChecker,
            IIocResolver iocResolver,
            IGeevStartupConfiguration startupConfiguration)
        {
            MultiTenancyConfig = multiTenancyConfig;
            LanguageManager = languageManager;
            LocalizationManager = localizationManager;
            FeatureManager = featureManager;
            FeatureChecker = featureChecker;
            PermissionManager = permissionManager;
            UserNavigationManager = userNavigationManager;
            SettingDefinitionManager = settingDefinitionManager;
            SettingManager = settingManager;
            GeevAntiForgeryConfiguration = geevAntiForgeryConfiguration;
            GeevSession = geevSession;
            PermissionChecker = permissionChecker;
            _iocResolver = iocResolver;
            _startupConfiguration = startupConfiguration;

            CustomDataConfig = new Dictionary<string, object>();
        }

        public virtual async Task<GeevUserConfigurationDto> GetAll()
        {
            return new GeevUserConfigurationDto
            {
                MultiTenancy = GetUserMultiTenancyConfig(),
                Session = GetUserSessionConfig(),
                Localization = GetUserLocalizationConfig(),
                Features = await GetUserFeaturesConfig(),
                Auth = await GetUserAuthConfig(),
                Nav = await GetUserNavConfig(),
                Setting = await GetUserSettingConfig(),
                Clock = GetUserClockConfig(),
                Timing = await GetUserTimingConfig(),
                Security = GetUserSecurityConfig(),
                Custom = _startupConfiguration.GetCustomConfig()
            };
        }

        protected virtual GeevMultiTenancyConfigDto GetUserMultiTenancyConfig()
        {
            return new GeevMultiTenancyConfigDto
            {
                IsEnabled = MultiTenancyConfig.IsEnabled,
                IgnoreFeatureCheckForHostUsers = MultiTenancyConfig.IgnoreFeatureCheckForHostUsers
            };
        }

        protected virtual GeevUserSessionConfigDto GetUserSessionConfig()
        {
            return new GeevUserSessionConfigDto
            {
                UserId = GeevSession.UserId,
                TenantId = GeevSession.TenantId,
                ImpersonatorUserId = GeevSession.ImpersonatorUserId,
                ImpersonatorTenantId = GeevSession.ImpersonatorTenantId,
                MultiTenancySide = GeevSession.MultiTenancySide
            };
        }

        protected virtual GeevUserLocalizationConfigDto GetUserLocalizationConfig()
        {
            var currentCulture = CultureInfo.CurrentUICulture;
            var languages = LanguageManager.GetLanguages();

            var config = new GeevUserLocalizationConfigDto
            {
                CurrentCulture = new GeevUserCurrentCultureConfigDto
                {
                    Name = currentCulture.Name,
                    DisplayName = currentCulture.DisplayName
                },
                Languages = languages.ToList()
            };

            if (languages.Count > 0)
            {
                config.CurrentLanguage = LanguageManager.CurrentLanguage;
            }

            var sources = LocalizationManager.GetAllSources().OrderBy(s => s.Name).ToArray();
            config.Sources = sources.Select(s => new GeevLocalizationSourceDto
            {
                Name = s.Name,
                Type = s.GetType().Name
            }).ToList();

            config.Values = new Dictionary<string, Dictionary<string, string>>();
            foreach (var source in sources)
            {
                var stringValues = source.GetAllStrings(currentCulture).OrderBy(s => s.Name).ToList();
                var stringDictionary = stringValues
                    .ToDictionary(_ => _.Name, _ => _.Value);
                config.Values.Add(source.Name, stringDictionary);
            }

            return config;
        }

        protected virtual async Task<GeevUserFeatureConfigDto> GetUserFeaturesConfig()
        {
            var config = new GeevUserFeatureConfigDto()
            {
                AllFeatures = new Dictionary<string, GeevStringValueDto>()
            };

            var allFeatures = FeatureManager.GetAll().ToList();

            if (GeevSession.TenantId.HasValue)
            {
                var currentTenantId = GeevSession.GetTenantId();
                foreach (var feature in allFeatures)
                {
                    var value = await FeatureChecker.GetValueAsync(currentTenantId, feature.Name);
                    config.AllFeatures.Add(feature.Name, new GeevStringValueDto
                    {
                        Value = value
                    });
                }
            }
            else
            {
                foreach (var feature in allFeatures)
                {
                    config.AllFeatures.Add(feature.Name, new GeevStringValueDto
                    {
                        Value = feature.DefaultValue
                    });
                }
            }

            return config;
        }

        protected virtual async Task<GeevUserAuthConfigDto> GetUserAuthConfig()
        {
            var config = new GeevUserAuthConfigDto();

            var allPermissionNames = PermissionManager.GetAllPermissions(false).Select(p => p.Name).ToList();
            var grantedPermissionNames = new List<string>();

            if (GeevSession.UserId.HasValue)
            {
                foreach (var permissionName in allPermissionNames)
                {
                    if (await PermissionChecker.IsGrantedAsync(permissionName))
                    {
                        grantedPermissionNames.Add(permissionName);
                    }
                }
            }

            config.AllPermissions = allPermissionNames.ToDictionary(permissionName => permissionName, permissionName => "true");
            config.GrantedPermissions = grantedPermissionNames.ToDictionary(permissionName => permissionName, permissionName => "true");

            return config;
        }

        protected virtual async Task<GeevUserNavConfigDto> GetUserNavConfig()
        {
            var userMenus = await UserNavigationManager.GetMenusAsync(GeevSession.ToUserIdentifier());
            return new GeevUserNavConfigDto
            {
                Menus = userMenus.ToDictionary(userMenu => userMenu.Name, userMenu => userMenu)
            };
        }

        protected virtual async Task<GeevUserSettingConfigDto> GetUserSettingConfig()
        {
            var config = new GeevUserSettingConfigDto
            {
                Values = new Dictionary<string, string>()
            };

            var settingDefinitions = SettingDefinitionManager
                .GetAllSettingDefinitions();

            using (var scope = _iocResolver.CreateScope())
            {
                foreach (var settingDefinition in settingDefinitions)
                {
                    if (!await settingDefinition.ClientVisibilityProvider.CheckVisible(scope))
                    {
                        continue;
                    }

                    var settingValue = await SettingManager.GetSettingValueAsync(settingDefinition.Name);
                    config.Values.Add(settingDefinition.Name, settingValue);
                }
            }

            return config;
        }

        protected virtual GeevUserClockConfigDto GetUserClockConfig()
        {
            return new GeevUserClockConfigDto
            {
                Provider = Clock.Provider.GetType().Name.ToCamelCase()
            };
        }

        protected virtual async Task<GeevUserTimingConfigDto> GetUserTimingConfig()
        {
            var timezoneId = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);
            var timezone = TimezoneHelper.FindTimeZoneInfo(timezoneId);

            return new GeevUserTimingConfigDto
            {
                TimeZoneInfo = new GeevUserTimeZoneConfigDto
                {
                    Windows = new GeevUserWindowsTimeZoneConfigDto
                    {
                        TimeZoneId = timezoneId,
                        BaseUtcOffsetInMilliseconds = timezone.BaseUtcOffset.TotalMilliseconds,
                        CurrentUtcOffsetInMilliseconds = timezone.GetUtcOffset(Clock.Now).TotalMilliseconds,
                        IsDaylightSavingTimeNow = timezone.IsDaylightSavingTime(Clock.Now)
                    },
                    Iana = new GeevUserIanaTimeZoneConfigDto
                    {
                        TimeZoneId = TimezoneHelper.WindowsToIana(timezoneId)
                    }
                }
            };
        }

        protected virtual GeevUserSecurityConfigDto GetUserSecurityConfig()
        {
            return new GeevUserSecurityConfigDto
            {
                AntiForgery = new GeevUserAntiForgeryConfigDto
                {
                    TokenCookieName = GeevAntiForgeryConfiguration.TokenCookieName,
                    TokenHeaderName = GeevAntiForgeryConfiguration.TokenHeaderName
                }
            };
        }
    }
}
