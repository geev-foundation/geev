using System.Threading.Tasks;
using Geev.Dependency;
using Geev.Runtime.Session;

namespace Geev.Configuration
{
    public class RequiresAuthenticationSettingClientVisibilityProvider : ISettingClientVisibilityProvider
    {
        public async Task<bool> CheckVisible(IScopedIocResolver scope)
        {
            return await Task.FromResult(
                scope.Resolve<IGeevSession>().UserId.HasValue
            );
        }
    }
}