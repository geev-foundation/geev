﻿using Geev.Dependency;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Castle.Windsor.MsDependencyInjection;

namespace Geev.AspNetCore
{
    public class GeevAspNetCoreConventionalRegistrar : IConventionalDependencyRegistrar
    {
        public void RegisterAssembly(IConventionalRegistrationContext context)
        {
            //ViewComponents
            context.IocManager.IocContainer.Register(
                Classes.FromAssembly(context.Assembly)
                    .BasedOn<ViewComponent>()
                    .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                    .LifestyleTransient()
            );

            //PerWebRequest
            context.IocManager.IocContainer.Register(
                Classes.FromAssembly(context.Assembly)
                    .IncludeNonPublicTypes()
                    .BasedOn<IPerWebRequestDependency>()
                    .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                    .WithService.Self()
                    .WithService.DefaultInterfaces()
                    .LifestyleCustom<MsScopedLifestyleManager>()
            );
        }
    }
}
