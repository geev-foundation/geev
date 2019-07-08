using Geev.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Geev.AspNetCore.App.Controllers
{
    public class NameConflictController : GeevController
    {
        public string GetSelfActionUrl()
        {
            return Url.Action("GetSelfActionUrl", "NameConflict");
        }

        public string GetAppServiceActionUrlWithArea()
        {
            //Gets URL of NameConflictAppService.GetConstantString action
            return Url.Action("GetConstantString", "NameConflict", new { area = "app"});
        }
    }
}
