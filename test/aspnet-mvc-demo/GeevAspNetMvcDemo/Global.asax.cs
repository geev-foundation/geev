using System;
using Geev.Web;

namespace GeevAspNetMvcDemo
{
    public class MvcApplication : GeevWebApplication<GeevAspNetMvcDemoModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);
        }

        protected override void Application_BeginRequest(object sender, EventArgs e)
        {
            base.Application_BeginRequest(sender, e);
        }
    }
}
