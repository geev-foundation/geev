using System.Threading.Tasks;
using Geev.Dependency;

namespace Geev.Configuration
{
    public interface ISettingClientVisibilityProvider
    {
        Task<bool> CheckVisible(IScopedIocResolver scope);
    }
}