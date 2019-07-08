using System;
using Geev.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Geev.AspNetCore.Mvc.Results.Wrapping
{
    public class GeevJsonActionResultWrapper : IGeevActionResultWrapper
    {
        public void Wrap(ResultExecutingContext actionResult)
        {
            var jsonResult = actionResult.Result as JsonResult;
            if (jsonResult == null)
            {
                throw new ArgumentException($"{nameof(actionResult)} should be JsonResult!");
            }

            if (!(jsonResult.Value is AjaxResponseBase))
            {
                jsonResult.Value = new AjaxResponse(jsonResult.Value);
            }
        }
    }
}