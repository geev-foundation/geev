using System;
using System.IO;
using System.Linq.Expressions;
using Geev.Application.Features;
using Geev.Application.Navigation;
using Geev.Application.Services;
using Geev.Auditing;
using Geev.Authorization;
using Geev.BackgroundJobs;
using Geev.Collections.Extensions;
using Geev.Configuration;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Domain.Uow;
using Geev.EntityHistory;
using Geev.Events.Bus;
using Geev.Localization;
using Geev.Localization.Dictionaries;
using Geev.Localization.Dictionaries.Xml;
using Geev.Modules;
using Geev.MultiTenancy;
using Geev.Net.Mail;
using Geev.Notifications;
using Geev.RealTime;
using Geev.Reflection.Extensions;
using Geev.Runtime;
using Geev.Runtime.Caching;
using Geev.Runtime.Remoting;
using Geev.Runtime.Validation.Interception;
using Geev.Threading;
using Geev.Threading.BackgroundWorkers;
using Geev.Timing;
using Castle.MicroKernel.Registration;

namespace Geev
{
    /// <summary>
    /// Kernel (core) module of the ABP system.
    /// No need to depend on this, it's automatically the first module always.
    /// </summary>
    public sealed class GeevKernelModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new BasicConventionalRegistrar());

            IocManager.Register<IScopedIocResolver, ScopedIocResolver>(DependencyLifeStyle.Transient);
            IocManager.Register(typeof(IAmbientScopeProvider<>), typeof(DataContextAmbientScopeProvider<>), DependencyLifeStyle.Transient);

            AddAuditingSelectors();
            AddLocalizationSources();
            AddSettingProviders();
            AddUnitOfWorkFilters();
            ConfigureCaches();
            AddIgnoredTypes();
            AddMethodParameterValidators();
            AddDefaultNotificationDistributor();
        }

        public override void Initialize()
        {
            foreach (var replaceAction in ((GeevStartupConfiguration)Configuration).ServiceReplaceActions.Values)
            {
                replaceAction();
            }

            IocManager.IocContainer.Install(new EventBusInstaller(IocManager));

            IocManager.Register(typeof(IOnlineClientManager<>), typeof(OnlineClientManager<>), DependencyLifeStyle.Singleton);

            IocManager.Register(typeof(EventTriggerAsyncBackgroundJob<>),DependencyLifeStyle.Transient);
            
            IocManager.RegisterAssemblyByConvention(typeof(GeevKernelModule).GetAssembly(),
                new ConventionalRegistrationConfig
                {
                    InstallInstallers = false
                });
            
        }

        public override void PostInitialize()
        {
            RegisterMissingComponents();

            IocManager.Resolve<SettingDefinitionManager>().Initialize();
            IocManager.Resolve<FeatureManager>().Initialize();
            IocManager.Resolve<PermissionManager>().Initialize();
            IocManager.Resolve<LocalizationManager>().Initialize();
            IocManager.Resolve<NotificationDefinitionManager>().Initialize();
            IocManager.Resolve<NavigationManager>().Initialize();

            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                var workerManager = IocManager.Resolve<IBackgroundWorkerManager>();
                workerManager.Start();
                workerManager.Add(IocManager.Resolve<IBackgroundJobManager>());
            }
        }

        public override void Shutdown()
        {
            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                IocManager.Resolve<IBackgroundWorkerManager>().StopAndWaitToStop();
            }
        }

        private void AddUnitOfWorkFilters()
        {
            Configuration.UnitOfWork.RegisterFilter(GeevDataFilters.SoftDelete, true);
            Configuration.UnitOfWork.RegisterFilter(GeevDataFilters.MustHaveTenant, true);
            Configuration.UnitOfWork.RegisterFilter(GeevDataFilters.MayHaveTenant, true);
        }

        private void AddSettingProviders()
        {
            Configuration.Settings.Providers.Add<LocalizationSettingProvider>();
            Configuration.Settings.Providers.Add<EmailSettingProvider>();
            Configuration.Settings.Providers.Add<NotificationSettingProvider>();
            Configuration.Settings.Providers.Add<TimingSettingProvider>();
        }

        private void AddAuditingSelectors()
        {
            Configuration.Auditing.Selectors.Add(
                new NamedTypeSelector(
                    "Geev.ApplicationServices",
                    type => typeof(IApplicationService).IsAssignableFrom(type)
                )
            );
        }

        private void AddLocalizationSources()
        {
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    GeevConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(GeevKernelModule).GetAssembly(), "Geev.Localization.Sources.GeevXmlSource"
                    )));
        }

        private void ConfigureCaches()
        {
            Configuration.Caching.Configure(GeevCacheNames.ApplicationSettings, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromHours(8);
            });

            Configuration.Caching.Configure(GeevCacheNames.TenantSettings, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(60);
            });

            Configuration.Caching.Configure(GeevCacheNames.UserSettings, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(20);
            });
        }

        private void AddIgnoredTypes()
        {
            var commonIgnoredTypes = new[]
            {
                typeof(Stream),
                typeof(Expression)
            };

            foreach (var ignoredType in commonIgnoredTypes)
            {
                Configuration.Auditing.IgnoredTypes.AddIfNotContains(ignoredType);
                Configuration.Validation.IgnoredTypes.AddIfNotContains(ignoredType);
            }

            var validationIgnoredTypes = new[] { typeof(Type) };
            foreach (var ignoredType in validationIgnoredTypes)
            {
                Configuration.Validation.IgnoredTypes.AddIfNotContains(ignoredType);
            }
        }

        private void AddMethodParameterValidators()
        {
            Configuration.Validation.Validators.Add<DataAnnotationsValidator>();
            Configuration.Validation.Validators.Add<ValidatableObjectValidator>();
            Configuration.Validation.Validators.Add<CustomValidator>();
        }

        private void AddDefaultNotificationDistributor()
        {
            Configuration.Notifications.Distributers.Add<DefaultNotificationDistributer>();
        }

        private void RegisterMissingComponents()
        {
            if (!IocManager.IsRegistered<IGuidGenerator>())
            {
                IocManager.IocContainer.Register(
                    Component
                        .For<IGuidGenerator, SequentialGuidGenerator>()
                        .Instance(SequentialGuidGenerator.Instance)
                );
            }

            IocManager.RegisterIfNot<IUnitOfWork, NullUnitOfWork>(DependencyLifeStyle.Transient);
            IocManager.RegisterIfNot<IAuditingStore, SimpleLogAuditingStore>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IPermissionChecker, NullPermissionChecker>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IRealTimeNotifier, NullRealTimeNotifier>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<INotificationStore, NullNotificationStore>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IUnitOfWorkFilterExecuter, NullUnitOfWorkFilterExecuter>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IClientInfoProvider, NullClientInfoProvider>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<ITenantStore, NullTenantStore>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<ITenantResolverCache, NullTenantResolverCache>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IEntityHistoryStore, NullEntityHistoryStore>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IOnlineClientStore, InMemoryOnlineClientStore>(DependencyLifeStyle.Singleton);

            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                IocManager.RegisterIfNot<IBackgroundJobStore, InMemoryBackgroundJobStore>(DependencyLifeStyle.Singleton);
            }
            else
            {
                IocManager.RegisterIfNot<IBackgroundJobStore, NullBackgroundJobStore>(DependencyLifeStyle.Singleton);
            }
        }
    }
}