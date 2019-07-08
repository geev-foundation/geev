using System.Threading.Tasks;
using Geev.Authorization;
using Geev.Dependency;
using Geev.Runtime.Session;

namespace Geev.Configuration
{
    public class RequiresPermissionSettingClientVisibilityProvider : ISettingClientVisibilityProvider
    {
        private readonly IPermissionDependency _permissionDependency;

        public RequiresPermissionSettingClientVisibilityProvider(IPermissionDependency permissionDependency)
        {
            _permissionDependency = permissionDependency;
        }

        public async Task<bool> CheckVisible(IScopedIocResolver scope)
        {
            var geevSession = scope.Resolve<IGeevSession>();

            if (!geevSession.UserId.HasValue)
            {
                return false;
            }

            var permissionDependencyContext = scope.Resolve<PermissionDependencyContext>();
            permissionDependencyContext.User = geevSession.ToUserIdentifier();

            return await _permissionDependency.IsSatisfiedAsync(permissionDependencyContext);
        }
    }
}