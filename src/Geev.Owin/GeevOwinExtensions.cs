using System;
using System.Web;
using Geev.Dependency;
using Geev.Modules;
using Geev.Owin.EmbeddedResources;
using Geev.Resources.Embedded;
using Geev.Threading;
using Geev.Web.Configuration;
using JetBrains.Annotations;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace Geev.Owin
{
    /// <summary>
    /// OWIN extension methods for ABP.
    /// </summary>
    public static class GeevOwinExtensions
    {
        /// <summary>
        /// This should be called as the first line for OWIN based applications for ABP framework.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void UseGeev(this IAppBuilder app)
        {
            app.UseGeev(null);
        }

        public static void UseGeev(this IAppBuilder app, [CanBeNull] Action<GeevOwinOptions> optionsAction)
        {
            ThreadCultureSanitizer.Sanitize();

            var options = new GeevOwinOptions
            {
                UseEmbeddedFiles = HttpContext.Current?.Server != null
            };

            optionsAction?.Invoke(options);

            if (options.UseEmbeddedFiles)
            {
                if (HttpContext.Current?.Server == null)
                {
                    throw new GeevInitializationException("Can not enable UseEmbeddedFiles for OWIN since HttpContext.Current is null! If you are using ASP.NET Core, serve embedded resources through ASP.NET Core middleware instead of OWIN. See http://www.aspnetboilerplate.com/Pages/Documents/Embedded-Resource-Files#aspnet-core-configuration");
                }

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileSystem = new GeevOwinEmbeddedResourceFileSystem(
                        IocManager.Instance.Resolve<IEmbeddedResourceManager>(),
                        IocManager.Instance.Resolve<IWebEmbeddedResourcesConfiguration>(),
                        HttpContext.Current.Server.MapPath("~/")
                    )
                });
            }
        }

        /// <summary>
        /// Use this extension method if you don't initialize ABP in other way.
        /// Otherwise, use <see cref="UseGeev(IAppBuilder)"/>.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <typeparam name="TStartupModule">The type of the startup module.</typeparam>
        public static void UseGeev<TStartupModule>(this IAppBuilder app)
            where TStartupModule : GeevModule
        {
            app.UseGeev<TStartupModule>(null, null);
        }

        /// <summary>
        /// Use this extension method if you don't initialize ABP in other way.
        /// Otherwise, use <see cref="UseGeev(IAppBuilder)"/>.
        /// </summary>
        /// <typeparam name="TStartupModule">The type of the startup module.</typeparam>
        public static void UseGeev<TStartupModule>(this IAppBuilder app, [CanBeNull] Action<GeevBootstrapper> configureAction, [CanBeNull] Action<GeevOwinOptions> optionsAction = null)
            where TStartupModule : GeevModule
        {
            app.UseGeev(optionsAction);

            if (!app.Properties.ContainsKey("_GeevBootstrapper.Instance"))
            {
                var geevBootstrapper = GeevBootstrapper.Create<TStartupModule>();
                app.Properties["_GeevBootstrapper.Instance"] = geevBootstrapper;
                configureAction?.Invoke(geevBootstrapper);
                geevBootstrapper.Initialize();
            }
        }
    }
}