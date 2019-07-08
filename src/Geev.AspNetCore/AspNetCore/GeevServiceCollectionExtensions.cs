using System;
using Geev.AspNetCore.EmbeddedResources;
using Geev.AspNetCore.Mvc;
using Geev.AspNetCore.Mvc.Antiforgery;
using Geev.Dependency;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Geev.AspNetCore.Mvc.Providers;
using Geev.Json;
using Geev.Modules;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace Geev.AspNetCore
{
    public static class GeevServiceCollectionExtensions
    {
        /// <summary>
        /// Integrates ABP to AspNet Core.
        /// </summary>
        /// <typeparam name="TStartupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="GeevModule"/>.</typeparam>
        /// <param name="services">Services.</param>
        /// <param name="optionsAction">An action to get/modify options</param>
        public static IServiceProvider AddGeev<TStartupModule>(this IServiceCollection services, [CanBeNull] Action<GeevBootstrapperOptions> optionsAction = null)
            where TStartupModule : GeevModule
        {
            var geevBootstrapper = AddGeevBootstrapper<TStartupModule>(services, optionsAction);

            ConfigureAspNetCore(services, geevBootstrapper.IocManager);

            return WindsorRegistrationHelper.CreateServiceProvider(geevBootstrapper.IocManager.IocContainer, services);
        }

        private static void ConfigureAspNetCore(IServiceCollection services, IIocResolver iocResolver)
        {
            //See https://github.com/aspnet/Mvc/issues/3936 to know why we added these services.
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            
            //Use DI to create controllers
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            //Use DI to create view components
            services.Replace(ServiceDescriptor.Singleton<IViewComponentActivator, ServiceBasedViewComponentActivator>());

            //Change anti forgery filters (to work proper with non-browser clients)
            services.Replace(ServiceDescriptor.Transient<AutoValidateAntiforgeryTokenAuthorizationFilter, GeevAutoValidateAntiforgeryTokenAuthorizationFilter>());
            services.Replace(ServiceDescriptor.Transient<ValidateAntiforgeryTokenAuthorizationFilter, GeevValidateAntiforgeryTokenAuthorizationFilter>());

            //Add feature providers
            var partManager = services.GetSingletonServiceOrNull<ApplicationPartManager>();
            partManager?.FeatureProviders.Add(new GeevAppServiceControllerFeatureProvider(iocResolver));

            //Configure JSON serializer
            services.Configure<MvcJsonOptions>(jsonOptions =>
            {
                jsonOptions.SerializerSettings.ContractResolver = new GeevMvcContractResolver(iocResolver)
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });

            //Configure MVC
            services.Configure<MvcOptions>(mvcOptions =>
            {
                mvcOptions.AddGeev(services);
            });

            //Configure Razor
            services.Insert(0,
                ServiceDescriptor.Singleton<IConfigureOptions<RazorViewEngineOptions>>(
                    new ConfigureOptions<RazorViewEngineOptions>(
                        (options) =>
                        {
                            options.FileProviders.Add(new EmbeddedResourceViewFileProvider(iocResolver));
                        }
                    )
                )
            );
        }

        private static GeevBootstrapper AddGeevBootstrapper<TStartupModule>(IServiceCollection services, Action<GeevBootstrapperOptions> optionsAction)
            where TStartupModule : GeevModule
        {
            var geevBootstrapper = GeevBootstrapper.Create<TStartupModule>(optionsAction);
            services.AddSingleton(geevBootstrapper);
            return geevBootstrapper;
        }
    }
}