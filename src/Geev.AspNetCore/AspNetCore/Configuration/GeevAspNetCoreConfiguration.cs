using System;
using System.Collections.Generic;
using System.Reflection;
using Geev.AspNetCore.Mvc.Results.Caching;
using Geev.Domain.Uow;
using Geev.Web.Models;
using Microsoft.AspNetCore.Routing;

namespace Geev.AspNetCore.Configuration
{
    public class GeevAspNetCoreConfiguration : IGeevAspNetCoreConfiguration
    {
        public WrapResultAttribute DefaultWrapResultAttribute { get; }

        public IClientCacheAttribute DefaultClientCacheAttribute { get; set; }

        public UnitOfWorkAttribute DefaultUnitOfWorkAttribute { get; }

        public List<Type> FormBodyBindingIgnoredTypes { get; }

        public ControllerAssemblySettingList ControllerAssemblySettings { get; }

        public bool IsValidationEnabledForControllers { get; set; }

        public bool IsAuditingEnabled { get; set; }

        public bool SetNoCacheForAjaxResponses { get; set; }

        public bool UseMvcDateTimeFormatForAppServices { get; set; }

        public List<Action<IRouteBuilder>> RouteConfiguration { get; }

        public GeevAspNetCoreConfiguration()
        {
            DefaultWrapResultAttribute = new WrapResultAttribute();
            DefaultClientCacheAttribute = new NoClientCacheAttribute(false);
            DefaultUnitOfWorkAttribute = new UnitOfWorkAttribute();
            ControllerAssemblySettings = new ControllerAssemblySettingList();
            FormBodyBindingIgnoredTypes = new List<Type>();
            RouteConfiguration = new List<Action<IRouteBuilder>>();
            IsValidationEnabledForControllers = true;
            SetNoCacheForAjaxResponses = true;
            IsAuditingEnabled = true;
            UseMvcDateTimeFormatForAppServices = false;
        }
       
        public GeevControllerAssemblySettingBuilder CreateControllersForAppServices(
            Assembly assembly,
            string moduleName = GeevControllerAssemblySetting.DefaultServiceModuleName,
            bool useConventionalHttpVerbs = true)
        {
            var setting = new GeevControllerAssemblySetting(moduleName, assembly, useConventionalHttpVerbs);
            ControllerAssemblySettings.Add(setting);
            return new GeevControllerAssemblySettingBuilder(setting);
        }
    }
}