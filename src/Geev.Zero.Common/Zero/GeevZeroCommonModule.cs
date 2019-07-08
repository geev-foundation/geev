using System.Linq;
using System.Reflection;
using Geev.Application.Features;
using Geev.Auditing;
using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.Collections.Extensions;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Localization;
using Geev.Localization.Dictionaries;
using Geev.Localization.Dictionaries.Xml;
using Geev.Modules;
using Geev.MultiTenancy;
using Geev.Reflection;
using Geev.Reflection.Extensions;
using Geev.Zero.Configuration;
using Castle.MicroKernel.Registration;

namespace Geev.Zero
{
    /// <summary>
    /// ABP zero core module.
    /// </summary>
    [DependsOn(typeof(GeevKernelModule))]
    public class GeevZeroCommonModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.RegisterIfNot<IGeevZeroEntityTypes, GeevZeroEntityTypes>(); //Registered on services.AddGeevIdentity() for Geev.ZeroCore.

            IocManager.Register<IRoleManagementConfig, RoleManagementConfig>();
            IocManager.Register<IUserManagementConfig, UserManagementConfig>();
            IocManager.Register<ILanguageManagementConfig, LanguageManagementConfig>();
            IocManager.Register<IGeevZeroConfig, GeevZeroConfig>();

            Configuration.ReplaceService<ITenantStore, TenantStore>(DependencyLifeStyle.Transient);

            Configuration.Settings.Providers.Add<GeevZeroSettingProvider>();

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    GeevZeroConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(GeevZeroCommonModule).GetAssembly(), "Geev.Zero.Localization.Source"
                        )));

            IocManager.IocContainer.Kernel.ComponentRegistered += Kernel_ComponentRegistered;

            AddIgnoredTypes();
        }

        public override void Initialize()
        {
            FillMissingEntityTypes();

            IocManager.Register<IMultiTenantLocalizationDictionary, MultiTenantLocalizationDictionary>(DependencyLifeStyle.Transient);
            IocManager.RegisterAssemblyByConvention(typeof(GeevZeroCommonModule).GetAssembly());

            RegisterTenantCache();
        }

        private void Kernel_ComponentRegistered(string key, Castle.MicroKernel.IHandler handler)
        {
            if (typeof(IGeevZeroFeatureValueStore).IsAssignableFrom(handler.ComponentModel.Implementation) && !IocManager.IsRegistered<IGeevZeroFeatureValueStore>())
            {
                IocManager.IocContainer.Register(
                    Component.For<IGeevZeroFeatureValueStore>().ImplementedBy(handler.ComponentModel.Implementation).Named("GeevZeroFeatureValueStore").LifestyleTransient()
                    );
            }
        }

        private void AddIgnoredTypes()
        {
            var ignoredTypes = new[]
            {
                typeof(AuditLog)
            };

            foreach (var ignoredType in ignoredTypes)
            {
                Configuration.EntityHistory.IgnoredTypes.AddIfNotContains(ignoredType);
            }
        }

        private void FillMissingEntityTypes()
        {
            using (var entityTypes = IocManager.ResolveAsDisposable<IGeevZeroEntityTypes>())
            {
                if (entityTypes.Object.User != null &&
                    entityTypes.Object.Role != null &&
                    entityTypes.Object.Tenant != null)
                {
                    return;
                }

                using (var typeFinder = IocManager.ResolveAsDisposable<ITypeFinder>())
                {
                    var types = typeFinder.Object.FindAll();
                    entityTypes.Object.Tenant = types.FirstOrDefault(t => typeof(GeevTenantBase).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract);
                    entityTypes.Object.Role = types.FirstOrDefault(t => typeof(GeevRoleBase).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract);
                    entityTypes.Object.User = types.FirstOrDefault(t => typeof(GeevUserBase).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract);
                }
            }
        }

        private void RegisterTenantCache()
        {
            if (IocManager.IsRegistered<ITenantCache>())
            {
                return;
            }

            using (var entityTypes = IocManager.ResolveAsDisposable<IGeevZeroEntityTypes>())
            {
                var implType = typeof (TenantCache<,>)
                    .MakeGenericType(entityTypes.Object.Tenant, entityTypes.Object.User);

                IocManager.Register(typeof (ITenantCache), implType, DependencyLifeStyle.Transient);
            }
        }
    }
}
