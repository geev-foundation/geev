using Geev.Dependency;

namespace Geev.Domain.Services
{
    /// <summary>
    /// This interface must be implemented by all domain services to identify them by convention.
    /// </summary>
    public interface IDomainService : ITransientDependency
    {

    }
}