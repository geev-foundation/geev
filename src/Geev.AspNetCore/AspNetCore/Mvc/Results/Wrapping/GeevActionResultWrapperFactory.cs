using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Geev.AspNetCore.Mvc.Results.Wrapping
{
    public class GeevActionResultWrapperFactory : IGeevActionResultWrapperFactory
    {
        public IGeevActionResultWrapper CreateFor(ResultExecutingContext actionResult)
        {
            Check.NotNull(actionResult, nameof(actionResult));

            if (actionResult.Result is ObjectResult)
            {
                return new GeevObjectActionResultWrapper();
            }

            if (actionResult.Result is JsonResult)
            {
                return new GeevJsonActionResultWrapper();
            }

            if (actionResult.Result is EmptyResult)
            {
                return new GeevEmptyActionResultWrapper();
            }

            return new NullGeevActionResultWrapper();
        }
    }
}