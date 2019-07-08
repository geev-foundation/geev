using System;
using System.IdentityModel.Tokens.Jwt;
using Geev.Authorization.Users;
using Geev.IdentityServer4;
using Geev.Runtime.Security;
using IdentityModel;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class GeevZeroIdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddGeevIdentityServer<TUser>(this IIdentityServerBuilder builder, Action<GeevIdentityServerOptions> optionsAction = null)
            where TUser : GeevUser<TUser>
        {
            var options = new GeevIdentityServerOptions();
            optionsAction?.Invoke(options);

            builder.AddAspNetIdentity<TUser>();

            builder.AddProfileService<GeevProfileService<TUser>>();
            builder.AddResourceOwnerValidator<GeevResourceOwnerPasswordValidator<TUser>>();

            builder.Services.Replace(ServiceDescriptor.Transient<IClaimsService, GeevClaimsService>());

            if (options.UpdateGeevClaimTypes)
            {
                GeevClaimTypes.UserId = JwtClaimTypes.Subject;
                GeevClaimTypes.UserName = JwtClaimTypes.Name;
                GeevClaimTypes.Role = JwtClaimTypes.Role;
            }

            if (options.UpdateJwtSecurityTokenHandlerDefaultInboundClaimTypeMap)
            {
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[GeevClaimTypes.UserId] = GeevClaimTypes.UserId;
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[GeevClaimTypes.UserName] = GeevClaimTypes.UserName;
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[GeevClaimTypes.Role] = GeevClaimTypes.Role;
            }

            return builder;
        }
    }
}
