using Geev.Collections;

namespace Geev.Zero.Configuration
{
    /// <summary>
    /// User management configuration.
    /// </summary>
    public interface IUserManagementConfig
    {
        ITypeList<object> ExternalAuthenticationSources { get; set; }
    }
}