using Microsoft.AspNetCore.Mvc.Filters;

namespace Geev.AspNetCore.Mvc.Results.Caching
{
    public interface IClientCacheAttribute
    {
        void Apply(ResultExecutingContext context);
    }
}