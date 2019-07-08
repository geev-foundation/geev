using Geev.Authorization.Users;
using Geev.Dependency;
using Geev.Extensions;
using Microsoft.Owin.Security.DataProtection;
using Owin;

namespace Geev.Owin
{
    public static class GeevZeroOwinAppBuilderExtensions
    {
        public static void RegisterDataProtectionProvider(this IAppBuilder app)
        {
            if (!IocManager.Instance.IsRegistered<IUserTokenProviderAccessor>())
            {
                throw new GeevException("IUserTokenProviderAccessor is not registered!");
            }

            var providerAccessor = IocManager.Instance.Resolve<IUserTokenProviderAccessor>();
            if (!(providerAccessor is OwinUserTokenProviderAccessor))
            {
                throw new GeevException($"IUserTokenProviderAccessor should be instance of {nameof(OwinUserTokenProviderAccessor)}!");
            }

            providerAccessor.As<OwinUserTokenProviderAccessor>().DataProtectionProvider = app.GetDataProtectionProvider();
        }
    }
}