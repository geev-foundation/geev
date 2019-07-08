using Geev.Localization;

namespace Geev.ZeroCore.SampleApp.Application
{
    public static class AppLocalizationHelper
    {
        public static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }
    }
}
