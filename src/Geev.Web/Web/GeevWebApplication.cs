using System;
using System.Web;
using Geev.Dependency;
using Geev.Modules;
using Geev.Threading;
using Geev.Web.Localization;

namespace Geev.Web
{
    /// <summary>
    /// This class is used to simplify starting of ABP system using <see cref="GeevBootstrapper"/> class..
    /// Inherit from this class in global.asax instead of <see cref="HttpApplication"/> to be able to start ABP system.
    /// </summary>
    /// <typeparam name="TStartupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="GeevModule"/>.</typeparam>
    public abstract class GeevWebApplication<TStartupModule> : HttpApplication
        where TStartupModule : GeevModule
    {
        /// <summary>
        /// Gets a reference to the <see cref="GeevBootstrapper"/> instance.
        /// </summary>
        public static GeevBootstrapper GeevBootstrapper { get; } = GeevBootstrapper.Create<TStartupModule>();

        protected virtual void Application_Start(object sender, EventArgs e)
        {
            ThreadCultureSanitizer.Sanitize();
            GeevBootstrapper.Initialize();
        }

        protected virtual void Application_End(object sender, EventArgs e)
        {
            GeevBootstrapper.Dispose();
        }

        protected virtual void Session_Start(object sender, EventArgs e)
        {

        }

        protected virtual void Session_End(object sender, EventArgs e)
        {

        }

        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
            
        }

        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected virtual void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            SetCurrentCulture();
        }

        protected virtual void Application_EndRequest(object sender, EventArgs e)
        {

        }

        protected virtual void Application_Error(object sender, EventArgs e)
        {

        }

        protected virtual void SetCurrentCulture()
        {
            GeevBootstrapper.IocManager.Using<ICurrentCultureSetter>(cultureSetter => cultureSetter.SetCurrentCulture(Context));
        }
    }
}
