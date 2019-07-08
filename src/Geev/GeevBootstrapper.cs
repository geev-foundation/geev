using System;
using System.Reflection;
using Geev.Auditing;
using Geev.Authorization;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Dependency.Installers;
using Geev.Domain.Uow;
using Geev.EntityHistory;
using Geev.Modules;
using Geev.PlugIns;
using Geev.Runtime.Validation.Interception;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using JetBrains.Annotations;

namespace Geev
{
    /// <summary>
    /// This is the main class that is responsible to start entire ABP system.
    /// Prepares dependency injection and registers core components needed for startup.
    /// It must be instantiated and initialized (see <see cref="Initialize"/>) first in an application.
    /// </summary>
    public class GeevBootstrapper : IDisposable
    {
        /// <summary>
        /// Get the startup module of the application which depends on other used modules.
        /// </summary>
        public Type StartupModule { get; }

        /// <summary>
        /// A list of plug in folders.
        /// </summary>
        public PlugInSourceList PlugInSources { get; }

        /// <summary>
        /// Gets IIocManager object used by this class.
        /// </summary>
        public IIocManager IocManager { get; }

        /// <summary>
        /// Is this object disposed before?
        /// </summary>
        protected bool IsDisposed;

        private GeevModuleManager _moduleManager;
        private ILogger _logger;

        /// <summary>
        /// Creates a new <see cref="GeevBootstrapper"/> instance.
        /// </summary>
        /// <param name="startupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="GeevModule"/>.</param>
        /// <param name="optionsAction">An action to set options</param>
        private GeevBootstrapper([NotNull] Type startupModule, [CanBeNull] Action<GeevBootstrapperOptions> optionsAction = null)
        {
            Check.NotNull(startupModule, nameof(startupModule));

            var options = new GeevBootstrapperOptions();
            optionsAction?.Invoke(options);

            if (!typeof(GeevModule).GetTypeInfo().IsAssignableFrom(startupModule))
            {
                throw new ArgumentException($"{nameof(startupModule)} should be derived from {nameof(GeevModule)}.");
            }

            StartupModule = startupModule;

            IocManager = options.IocManager;
            PlugInSources = options.PlugInSources;

            _logger = NullLogger.Instance;

            if (!options.DisableAllInterceptors)
            {
                AddInterceptorRegistrars();
            }
        }

        /// <summary>
        /// Creates a new <see cref="GeevBootstrapper"/> instance.
        /// </summary>
        /// <typeparam name="TStartupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="GeevModule"/>.</typeparam>
        /// <param name="optionsAction">An action to set options</param>
        public static GeevBootstrapper Create<TStartupModule>([CanBeNull] Action<GeevBootstrapperOptions> optionsAction = null)
            where TStartupModule : GeevModule
        {
            return new GeevBootstrapper(typeof(TStartupModule), optionsAction);
        }

        /// <summary>
        /// Creates a new <see cref="GeevBootstrapper"/> instance.
        /// </summary>
        /// <param name="startupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="GeevModule"/>.</param>
        /// <param name="optionsAction">An action to set options</param>
        public static GeevBootstrapper Create([NotNull] Type startupModule, [CanBeNull] Action<GeevBootstrapperOptions> optionsAction = null)
        {
            return new GeevBootstrapper(startupModule, optionsAction);
        }

        /// <summary>
        /// Creates a new <see cref="GeevBootstrapper"/> instance.
        /// </summary>
        /// <typeparam name="TStartupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="GeevModule"/>.</typeparam>
        /// <param name="iocManager">IIocManager that is used to bootstrap the ABP system</param>
        [Obsolete("Use overload with parameter type: Action<GeevBootstrapperOptions> optionsAction")]
        public static GeevBootstrapper Create<TStartupModule>([NotNull] IIocManager iocManager)
            where TStartupModule : GeevModule
        {
            return new GeevBootstrapper(typeof(TStartupModule), options =>
            {
                options.IocManager = iocManager;
            });
        }

        /// <summary>
        /// Creates a new <see cref="GeevBootstrapper"/> instance.
        /// </summary>
        /// <param name="startupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="GeevModule"/>.</param>
        /// <param name="iocManager">IIocManager that is used to bootstrap the ABP system</param>
        [Obsolete("Use overload with parameter type: Action<GeevBootstrapperOptions> optionsAction")]
        public static GeevBootstrapper Create([NotNull] Type startupModule, [NotNull] IIocManager iocManager)
        {
            return new GeevBootstrapper(startupModule, options =>
            {
                options.IocManager = iocManager;
            });
        }

        private void AddInterceptorRegistrars()
        {
            ValidationInterceptorRegistrar.Initialize(IocManager);
            AuditingInterceptorRegistrar.Initialize(IocManager);
            EntityHistoryInterceptorRegistrar.Initialize(IocManager);
            UnitOfWorkRegistrar.Initialize(IocManager);
            AuthorizationInterceptorRegistrar.Initialize(IocManager);
        }

        /// <summary>
        /// Initializes the ABP system.
        /// </summary>
        public virtual void Initialize()
        {
            ResolveLogger();

            try
            {
                RegisterBootstrapper();
                IocManager.IocContainer.Install(new GeevCoreInstaller());

                IocManager.Resolve<GeevPlugInManager>().PlugInSources.AddRange(PlugInSources);
                IocManager.Resolve<GeevStartupConfiguration>().Initialize();

                _moduleManager = IocManager.Resolve<GeevModuleManager>();
                _moduleManager.Initialize(StartupModule);
                _moduleManager.StartModules();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex.ToString(), ex);
                throw;
            }
        }

        private void ResolveLogger()
        {
            if (IocManager.IsRegistered<ILoggerFactory>())
            {
                _logger = IocManager.Resolve<ILoggerFactory>().Create(typeof(GeevBootstrapper));
            }
        }

        private void RegisterBootstrapper()
        {
            if (!IocManager.IsRegistered<GeevBootstrapper>())
            {
                IocManager.IocContainer.Register(
                    Component.For<GeevBootstrapper>().Instance(this)
                    );
            }
        }

        /// <summary>
        /// Disposes the ABP system.
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            _moduleManager?.ShutdownModules();
        }
    }
}
