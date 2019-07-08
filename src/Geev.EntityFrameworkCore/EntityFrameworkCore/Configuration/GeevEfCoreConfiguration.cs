using System;
using Geev.Dependency;
using Castle.MicroKernel.Registration;
using Microsoft.EntityFrameworkCore;

namespace Geev.EntityFrameworkCore.Configuration
{
    public class GeevEfCoreConfiguration : IGeevEfCoreConfiguration
    {
        private readonly IIocManager _iocManager;

        public GeevEfCoreConfiguration(IIocManager iocManager)
        {
            _iocManager = iocManager;
        }

        public void AddDbContext<TDbContext>(Action<GeevDbContextConfiguration<TDbContext>> action) 
            where TDbContext : DbContext
        {
            _iocManager.IocContainer.Register(
                Component.For<IGeevDbContextConfigurer<TDbContext>>().Instance(
                    new GeevDbContextConfigurerAction<TDbContext>(action)
                ).IsDefault()
            );
        }
    }
}