using Geev.Dependency;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Geev.AspNetCore.Mvc.Results.Wrapping
{
    public interface IGeevActionResultWrapperFactory : ITransientDependency
    {
        IGeevActionResultWrapper CreateFor([NotNull] ResultExecutingContext actionResult);
    }
}