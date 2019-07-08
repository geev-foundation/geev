namespace Geev.AspNetCore
{
    public class GeevApplicationBuilderOptions
    {
        /// <summary>
        /// Default: true.
        /// </summary>
        public bool UseCastleLoggerFactory { get; set; }

        /// <summary>
        /// Default: true.
        /// </summary>
        public bool UseGeevRequestLocalization { get; set; }

        /// <summary>
        /// Default: true.
        /// </summary>
        public bool UseSecurityHeaders { get; set; }

        public GeevApplicationBuilderOptions()
        {
            UseCastleLoggerFactory = true;
            UseGeevRequestLocalization = true;
            UseSecurityHeaders = true;
        }
    }
}