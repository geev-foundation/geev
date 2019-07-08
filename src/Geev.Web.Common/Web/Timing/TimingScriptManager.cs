using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Geev.Configuration;
using Geev.Dependency;
using Geev.Extensions;
using Geev.Timing;
using Geev.Timing.Timezone;

namespace Geev.Web.Timing
{
    /// <summary>
    /// This class is used to build timing script.
    /// </summary>
    public class TimingScriptManager : ITimingScriptManager, ITransientDependency
    {
        private readonly ISettingManager _settingManager;

        public TimingScriptManager(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        public async Task<string> GetScriptAsync()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");

            script.AppendLine("    geev.clock.provider = geev.timing." + Clock.Provider.GetType().Name.ToCamelCase() + " || geev.timing.localClockProvider;");
            script.AppendLine("    geev.clock.provider.supportsMultipleTimezone = " + Clock.SupportsMultipleTimezone.ToString().ToLowerInvariant() + ";");

            if (Clock.SupportsMultipleTimezone)
            {
                script.AppendLine("    geev.timing.timeZoneInfo = " + await GetUsersTimezoneScriptsAsync());
            }

            script.Append("})();");

            return script.ToString();
        }

        private async Task<string> GetUsersTimezoneScriptsAsync()
        {
            var timezoneId = await _settingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);
            var timezone = TimezoneHelper.FindTimeZoneInfo(timezoneId);

            return " {" +
                   "        windows: {" +
                   "            timeZoneId: '" + timezoneId + "'," +
                   "            baseUtcOffsetInMilliseconds: '" + timezone.BaseUtcOffset.TotalMilliseconds + "'," +
                   "            currentUtcOffsetInMilliseconds: '" + timezone.GetUtcOffset(Clock.Now).TotalMilliseconds + "'," +
                   "            isDaylightSavingTimeNow: '" + timezone.IsDaylightSavingTime(Clock.Now) + "'" +
                   "        }," +
                   "        iana: {" +
                   "            timeZoneId:'" + TimezoneHelper.WindowsToIana(timezoneId) + "'" +
                   "        }," +
                   "    }";
        }
    }
}