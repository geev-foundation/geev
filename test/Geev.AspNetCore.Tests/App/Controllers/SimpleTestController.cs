﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using Geev.AspNetCore.App.Models;
using Geev.AspNetCore.Mvc.Controllers;
using Geev.Timing;
using Geev.UI;
using Geev.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geev.AspNetCore.App.Controllers
{
    public class SimpleTestController : GeevController
    {
        public ActionResult SimpleContent()
        {
            return Content("Hello world...");
        }

        public JsonResult SimpleJson()
        {
            return Json(new SimpleViewModel("Forty Two", 42));
        }

        public JsonResult SimpleJsonException(string message, bool userFriendly)
        {
            if (userFriendly)
            {
                throw new UserFriendlyException(message);
            }

            throw new Exception(message);
        }

        [DontWrapResult]
        public JsonResult SimpleJsonExceptionDownWrap()
        {
            throw new UserFriendlyException("an exception message");
        }

        [DontWrapResult]
        public JsonResult SimpleJsonDontWrap()
        {
            return Json(new SimpleViewModel("Forty Two", 42));
        }

        [HttpGet]
        [WrapResult]
        public void GetVoidTest()
        {

        }

        [DontWrapResult]
        public void GetVoidTestDontWrap()
        {

        }

        [HttpGet]
        public ActionResult GetActionResultTest()
        {
            return Content("GetActionResultTest-Result");
        }

        [HttpGet]
        public async Task<ActionResult> GetActionResultTestAsync()
        {
            await Task.Delay(0);
            return Content("GetActionResultTestAsync-Result");
        }

        [HttpGet]
        public async Task GetVoidExceptionTestAsync()
        {
            await Task.Delay(0);
            throw new UserFriendlyException("GetVoidExceptionTestAsync-Exception");
        }

        [HttpGet]
        public async Task<ActionResult> GetActionResultExceptionTestAsync()
        {
            await Task.Delay(0);
            throw new UserFriendlyException("GetActionResultExceptionTestAsync-Exception");
        }

        [HttpGet]
        public ActionResult GetCurrentCultureNameTest()
        {
            return Content(CultureInfo.CurrentCulture.Name);
        }

        [HttpGet]
        public string GetDateTimeKind(SimpleDateModel input)
        {
            return input.Date.Kind.ToString().ToLower();
        }

        [HttpGet]
        public string GetNotNormalizedDateTimeKindProperty(SimpleDateModel2 input)
        {
            return input.Date.Kind.ToString();
        }


        [HttpGet]
        public SimpleDateModel2 GetNotNormalizedDateTimeKindProperty2(string date)
        {
            return new SimpleDateModel2
            {
                Date = Convert.ToDateTime(date)
            };
        }

        [HttpGet]
        public SimpleDateModel3 GetNotNormalizedDateTimeKindProperty3(string date)
        {
            return new SimpleDateModel3
            {
                Date = Convert.ToDateTime(date)
            };
        }

        [HttpGet]
        public SimpleDateModel4 GetNotNormalizedDateTimeKindProperty4([DisableDateTimeNormalization]DateTime date)
        {
            return new SimpleDateModel4
            {
                Date = date
            };
        }

        [HttpGet]
        public string GetNotNormalizedDateTimeKindClass(SimpleDateModel3 input)
        {
            return input.Date.Kind.ToString().ToLower();
        }
    }
}
