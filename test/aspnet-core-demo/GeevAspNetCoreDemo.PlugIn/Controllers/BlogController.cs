using Geev.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GeevAspNetCoreDemo.PlugIn.Controllers
{
    public class BlogController : GeevController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
