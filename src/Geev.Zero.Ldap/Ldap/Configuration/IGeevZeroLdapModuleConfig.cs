using System;

namespace Geev.Zero.Ldap.Configuration
{
    public interface IGeevZeroLdapModuleConfig
    {
        bool IsEnabled { get; }

        Type AuthenticationSourceType { get; }

        void Enable(Type authenticationSourceType);
    }
}