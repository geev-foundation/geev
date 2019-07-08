using System.Collections.Generic;

namespace Geev.Web.Models.GeevUserConfiguration
{
    public class GeevUserConfigurationDto
    {
        public GeevMultiTenancyConfigDto MultiTenancy { get; set; }

        public GeevUserSessionConfigDto Session { get; set; }

        public GeevUserLocalizationConfigDto Localization { get; set; }

        public GeevUserFeatureConfigDto Features { get; set; }

        public GeevUserAuthConfigDto Auth { get; set; }

        public GeevUserNavConfigDto Nav { get; set; }

        public GeevUserSettingConfigDto Setting { get; set; }

        public GeevUserClockConfigDto Clock { get; set; }

        public GeevUserTimingConfigDto Timing { get; set; }

        public GeevUserSecurityConfigDto Security { get; set; }

        public Dictionary<string, object> Custom { get; set; }
    }
}