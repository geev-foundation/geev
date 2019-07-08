using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace Geev.IdentityServer4
{
    public static class GeevZeroIdentityServerBuilderEntityFrameworkCoreExtensions
    {
        public static IIdentityServerBuilder AddGeevPersistedGrants<TDbContext>(this IIdentityServerBuilder builder)
            where TDbContext : IGeevPersistedGrantDbContext
        {
            builder.Services.AddTransient<IPersistedGrantStore, GeevPersistedGrantStore>();
            return builder;
        }
    }
}
