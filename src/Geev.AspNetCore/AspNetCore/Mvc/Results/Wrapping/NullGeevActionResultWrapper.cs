using Microsoft.AspNetCore.Mvc.Filters;

namespace Geev.AspNetCore.Mvc.Results.Wrapping
{
    public class NullGeevActionResultWrapper : IGeevActionResultWrapper
    {
        public void Wrap(ResultExecutingContext actionResult)
        {
            
        }
    }
}