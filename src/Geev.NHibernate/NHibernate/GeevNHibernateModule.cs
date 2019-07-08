using System.Reflection;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Domain.Uow;
using Geev.Modules;
using Geev.NHibernate.Configuration;
using Geev.NHibernate.Filters;
using Geev.NHibernate.Interceptors;
using Geev.NHibernate.Repositories;
using Geev.NHibernate.Uow;
using NHibernate;

namespace Geev.NHibernate
{
    /// <summary>
    /// This module is used to implement "Data Access Layer" in NHibernate.
    /// </summary>
    [DependsOn(typeof(GeevKernelModule))]
    public class GeevNHibernateModule : GeevModule
    {
        /// <summary>
        /// NHibernate session factory object.
        /// </summary>
        private ISessionFactory _sessionFactory;

        public override void PreInitialize()
        {
            IocManager.Register<IGeevNHibernateModuleConfiguration, GeevNHibernateModuleConfiguration>();
            Configuration.ReplaceService<IUnitOfWorkFilterExecuter, NhUnitOfWorkFilterExecuter>(DependencyLifeStyle.Transient);
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.Register<GeevNHibernateInterceptor>(DependencyLifeStyle.Transient);

            _sessionFactory = Configuration.Modules.GeevNHibernate().FluentConfiguration
                .Mappings(m => m.FluentMappings.Add(typeof(SoftDeleteFilter)))
                .Mappings(m => m.FluentMappings.Add(typeof(MayHaveTenantFilter)))
                .Mappings(m => m.FluentMappings.Add(typeof(MustHaveTenantFilter)))
                .ExposeConfiguration(config => config.SetInterceptor(IocManager.Resolve<GeevNHibernateInterceptor>()))
                .BuildSessionFactory();

            IocManager.IocContainer.Install(new NhRepositoryInstaller(_sessionFactory));
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        /// <inheritdoc/>
        public override void Shutdown()
        {
            _sessionFactory.Dispose();
        }
    }
}
