using System;
using Geev.Zero.Configuration;

namespace Geev.Zero.Ldap.Configuration
{
    public class GeevZeroLdapModuleConfig : IGeevZeroLdapModuleConfig
    {
        public bool IsEnabled { get; private set; }

        public Type AuthenticationSourceType { get; private set; }

        private readonly IGeevZeroConfig _zeroConfig;

        public GeevZeroLdapModuleConfig(IGeevZeroConfig zeroConfig)
        {
            _zeroConfig = zeroConfig;
        }

        public void Enable(Type authenticationSourceType)
        {
            AuthenticationSourceType = authenticationSourceType;
            IsEnabled = true;

            _zeroConfig.UserManagement.ExternalAuthenticationSources.Add(authenticationSourceType);
        }
    }
}