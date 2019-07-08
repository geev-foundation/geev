using System.Threading.Tasks;

namespace Geev.Application.Features
{
    /// <summary>
    /// Defines a feature dependency.
    /// </summary>
    public interface IFeatureDependency
    {
        /// <summary>
        /// Checks dependent features and returns true if the dependencies are satisfied.
        /// </summary>
        Task<bool> IsSatisfiedAsync(IFeatureDependencyContext context);
    }
}