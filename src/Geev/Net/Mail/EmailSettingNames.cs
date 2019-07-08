namespace Geev.Net.Mail
{
    /// <summary>
    /// Declares names of the settings defined by <see cref="EmailSettingProvider"/>.
    /// </summary>
    public static class EmailSettingNames
    {
        /// <summary>
        /// Geev.Net.Mail.DefaultFromAddress
        /// </summary>
        public const string DefaultFromAddress = "Geev.Net.Mail.DefaultFromAddress";

        /// <summary>
        /// Geev.Net.Mail.DefaultFromDisplayName
        /// </summary>
        public const string DefaultFromDisplayName = "Geev.Net.Mail.DefaultFromDisplayName";

        /// <summary>
        /// SMTP related email settings.
        /// </summary>
        public static class Smtp
        {
            /// <summary>
            /// Geev.Net.Mail.Smtp.Host
            /// </summary>
            public const string Host = "Geev.Net.Mail.Smtp.Host";

            /// <summary>
            /// Geev.Net.Mail.Smtp.Port
            /// </summary>
            public const string Port = "Geev.Net.Mail.Smtp.Port";

            /// <summary>
            /// Geev.Net.Mail.Smtp.UserName
            /// </summary>
            public const string UserName = "Geev.Net.Mail.Smtp.UserName";

            /// <summary>
            /// Geev.Net.Mail.Smtp.Password
            /// </summary>
            public const string Password = "Geev.Net.Mail.Smtp.Password";

            /// <summary>
            /// Geev.Net.Mail.Smtp.Domain
            /// </summary>
            public const string Domain = "Geev.Net.Mail.Smtp.Domain";

            /// <summary>
            /// Geev.Net.Mail.Smtp.EnableSsl
            /// </summary>
            public const string EnableSsl = "Geev.Net.Mail.Smtp.EnableSsl";

            /// <summary>
            /// Geev.Net.Mail.Smtp.UseDefaultCredentials
            /// </summary>
            public const string UseDefaultCredentials = "Geev.Net.Mail.Smtp.UseDefaultCredentials";
        }
    }
}