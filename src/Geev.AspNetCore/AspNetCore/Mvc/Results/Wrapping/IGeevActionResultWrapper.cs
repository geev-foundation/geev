using Microsoft.AspNetCore.Mvc.Filters;

namespace Geev.AspNetCore.Mvc.Results.Wrapping
{
    public interface IGeevActionResultWrapper
    {
        void Wrap(ResultExecutingContext actionResult);
    }
}