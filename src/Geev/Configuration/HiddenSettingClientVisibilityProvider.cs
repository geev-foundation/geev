using System.Threading.Tasks;
using Geev.Dependency;

namespace Geev.Configuration
{
    public class HiddenSettingClientVisibilityProvider : ISettingClientVisibilityProvider
    {
        public async Task<bool> CheckVisible(IScopedIocResolver scope)
        {
            return await Task.FromResult(false);
        }
    }
}