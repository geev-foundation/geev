using System.Threading.Tasks;
using Geev.Dependency;

namespace Geev.Configuration
{
    public class VisibleSettingClientVisibilityProvider : ISettingClientVisibilityProvider
    {
        public async Task<bool> CheckVisible(IScopedIocResolver scope)
        {
            return await Task.FromResult(true);
        }
    }
}