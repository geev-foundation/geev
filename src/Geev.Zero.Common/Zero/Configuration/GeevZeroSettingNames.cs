namespace Geev.Zero.Configuration
{
    public static class GeevZeroSettingNames
    {
        public static class UserManagement
        {
            /// <summary>
            /// "Geev.Zero.UserManagement.IsEmailConfirmationRequiredForLogin".
            /// </summary>
            public const string IsEmailConfirmationRequiredForLogin = "Geev.Zero.UserManagement.IsEmailConfirmationRequiredForLogin";

            public static class UserLockOut
            {
                /// <summary>
                /// "Geev.Zero.UserManagement.UserLockOut.IsEnabled".
                /// </summary>
                public const string IsEnabled = "Geev.Zero.UserManagement.UserLockOut.IsEnabled";

                /// <summary>
                /// "Geev.Zero.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout".
                /// </summary>
                public const string MaxFailedAccessAttemptsBeforeLockout = "Geev.Zero.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout";

                /// <summary>
                /// "Geev.Zero.UserManagement.UserLockOut.DefaultAccountLockoutSeconds".
                /// </summary>
                public const string DefaultAccountLockoutSeconds = "Geev.Zero.UserManagement.UserLockOut.DefaultAccountLockoutSeconds";
            }

            public static class TwoFactorLogin
            {
                /// <summary>
                /// "Geev.Zero.UserManagement.TwoFactorLogin.IsEnabled".
                /// </summary>
                public const string IsEnabled = "Geev.Zero.UserManagement.TwoFactorLogin.IsEnabled";

                /// <summary>
                /// "Geev.Zero.UserManagement.TwoFactorLogin.IsEmailProviderEnabled".
                /// </summary>
                public const string IsEmailProviderEnabled = "Geev.Zero.UserManagement.TwoFactorLogin.IsEmailProviderEnabled";

                /// <summary>
                /// "Geev.Zero.UserManagement.TwoFactorLogin.IsSmsProviderEnabled".
                /// </summary>
                public const string IsSmsProviderEnabled = "Geev.Zero.UserManagement.TwoFactorLogin.IsSmsProviderEnabled";

                /// <summary>
                /// "Geev.Zero.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled".
                /// </summary>
                public const string IsRememberBrowserEnabled = "Geev.Zero.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled";
            }

            public static class PasswordComplexity
            {
                /// <summary>
                /// "Geev.Zero.UserManagement.PasswordComplexity.RequiredLength"
                /// </summary>
                public const string RequiredLength = "Geev.Zero.UserManagement.PasswordComplexity.RequiredLength";

                /// <summary>
                /// "Geev.Zero.UserManagement.PasswordComplexity.RequireNonAlphanumeric"
                /// </summary>
                public const string RequireNonAlphanumeric = "Geev.Zero.UserManagement.PasswordComplexity.RequireNonAlphanumeric";

                /// <summary>
                /// "Geev.Zero.UserManagement.PasswordComplexity.RequireLowercase"
                /// </summary>
                public const string RequireLowercase = "Geev.Zero.UserManagement.PasswordComplexity.RequireLowercase";

                /// <summary>
                /// "Geev.Zero.UserManagement.PasswordComplexity.RequireUppercase"
                /// </summary>
                public const string RequireUppercase = "Geev.Zero.UserManagement.PasswordComplexity.RequireUppercase";

                /// <summary>
                /// "Geev.Zero.UserManagement.PasswordComplexity.RequireDigit"
                /// </summary>
                public const string RequireDigit = "Geev.Zero.UserManagement.PasswordComplexity.RequireDigit";
            }
        }

        public static class OrganizationUnits
        {
            /// <summary>
            /// "Geev.Zero.OrganizationUnits.MaxUserMembershipCount".
            /// </summary>
            public const string MaxUserMembershipCount = "Geev.Zero.OrganizationUnits.MaxUserMembershipCount";
        }
    }
}