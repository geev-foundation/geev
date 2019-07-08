namespace Geev.Web.Models.GeevUserConfiguration
{
    public class GeevUserWindowsTimeZoneConfigDto
    {
        public string TimeZoneId { get; set; }

        public double BaseUtcOffsetInMilliseconds { get; set; }

        public double CurrentUtcOffsetInMilliseconds { get; set; }

        public bool IsDaylightSavingTimeNow { get; set; }
    }
}