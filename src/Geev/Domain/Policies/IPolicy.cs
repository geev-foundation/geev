using Geev.Dependency;

namespace Geev.Domain.Policies
{
    /// <summary>
    /// This interface can be implemented by all Policy classes/interfaces to identify them by convention.
    /// </summary>
    public interface IPolicy : ITransientDependency
    {

    }
}
