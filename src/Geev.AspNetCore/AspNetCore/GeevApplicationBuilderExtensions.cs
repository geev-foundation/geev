using System;
using System.Linq;
using Geev.AspNetCore.EmbeddedResources;
using Geev.AspNetCore.Localization;
using Geev.Dependency;
using Geev.Localization;
using Castle.LoggingFacility.MsLogging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Geev.AspNetCore.Security;
using Microsoft.AspNetCore.Hosting;

namespace Geev.AspNetCore
{
    public static class GeevApplicationBuilderExtensions
    {
        public static void UseGeev(this IApplicationBuilder app)
        {
            app.UseGeev(null);
        }

        public static void UseGeev([NotNull] this IApplicationBuilder app, Action<GeevApplicationBuilderOptions> optionsAction)
        {
            Check.NotNull(app, nameof(app));

            var options = new GeevApplicationBuilderOptions();
            optionsAction?.Invoke(options);

            if (options.UseCastleLoggerFactory)
            {
                app.UseCastleLoggerFactory();
            }

            InitializeGeev(app);

            if (options.UseGeevRequestLocalization)
            {
                //TODO: This should be added later than authorization middleware!
                app.UseGeevRequestLocalization();
            }

            if (options.UseSecurityHeaders)
            {
                app.UseGeevSecurityHeaders();
            }
        }

        public static void UseEmbeddedFiles(this IApplicationBuilder app)
        {
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = new EmbeddedResourceFileProvider(
                        app.ApplicationServices.GetRequiredService<IIocResolver>()
                    )
                }
            );
        }

        private static void InitializeGeev(IApplicationBuilder app)
        {
            var geevBootstrapper = app.ApplicationServices.GetRequiredService<GeevBootstrapper>();
            geevBootstrapper.Initialize();

            var applicationLifetime = app.ApplicationServices.GetService<IApplicationLifetime>();
            applicationLifetime.ApplicationStopping.Register(() => geevBootstrapper.Dispose());
        }

        public static void UseCastleLoggerFactory(this IApplicationBuilder app)
        {
            var castleLoggerFactory = app.ApplicationServices.GetService<Castle.Core.Logging.ILoggerFactory>();
            if (castleLoggerFactory == null)
            {
                return;
            }

            app.ApplicationServices
                .GetRequiredService<ILoggerFactory>()
                .AddCastleLogger(castleLoggerFactory);
        }

        public static void UseGeevRequestLocalization(this IApplicationBuilder app, Action<RequestLocalizationOptions> optionsAction = null)
        {
            var iocResolver = app.ApplicationServices.GetRequiredService<IIocResolver>();
            using (var languageManager = iocResolver.ResolveAsDisposable<ILanguageManager>())
            {
                var supportedCultures = languageManager.Object
                    .GetLanguages()
                    .Select(l => CultureInfo.GetCultureInfo(l.Name))
                    .ToArray();

                var options = new RequestLocalizationOptions
                {
                    SupportedCultures = supportedCultures,
                    SupportedUICultures = supportedCultures
                };

                var userProvider = new GeevUserRequestCultureProvider();

                //0: QueryStringRequestCultureProvider
                options.RequestCultureProviders.Insert(1, userProvider);
                options.RequestCultureProviders.Insert(2, new GeevLocalizationHeaderRequestCultureProvider());
                //3: CookieRequestCultureProvider
                options.RequestCultureProviders.Insert(4, new GeevDefaultRequestCultureProvider());
                //5: AcceptLanguageHeaderRequestCultureProvider

                optionsAction?.Invoke(options);

                userProvider.CookieProvider = options.RequestCultureProviders.OfType<CookieRequestCultureProvider>().FirstOrDefault();
                userProvider.HeaderProvider = options.RequestCultureProviders.OfType<GeevLocalizationHeaderRequestCultureProvider>().FirstOrDefault();

                app.UseRequestLocalization(options);
            }
        }

        public static void UseGeevSecurityHeaders(this IApplicationBuilder app)
        {
            app.UseMiddleware<GeevSecurityHeadersMiddleware>();
        }
    }
}
