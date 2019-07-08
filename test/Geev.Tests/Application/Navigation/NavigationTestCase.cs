using System.Threading.Tasks;
using Geev.Application.Features;
using Geev.Application.Navigation;
using Geev.Authorization;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.Localization;
using Castle.MicroKernel.Registration;
using NSubstitute;

namespace Geev.Tests.Application.Navigation
{
    internal class NavigationTestCase
    {
        public NavigationManager NavigationManager { get; private set; }

        public UserNavigationManager UserNavigationManager { get; private set; }

        private readonly IIocManager _iocManager;

        public NavigationTestCase()
            : this(new IocManager())
        {
        }

        public NavigationTestCase(IIocManager iocManager)
        {
            _iocManager = iocManager;
            Initialize();
        }

        private void Initialize()
        {
            //Navigation providers should be registered
            _iocManager.Register<MyNavigationProvider1>();
            _iocManager.Register<MyNavigationProvider2>();

            //Preparing navigation configuration
            var configuration = new NavigationConfiguration();
            configuration.Providers.Add<MyNavigationProvider1>();
            configuration.Providers.Add<MyNavigationProvider2>();

            //Initializing navigation manager
            NavigationManager = new NavigationManager(_iocManager, configuration);
            NavigationManager.Initialize();

            _iocManager.IocContainer.Register(
                Component.For<IPermissionDependencyContext, PermissionDependencyContext>()
                    .UsingFactoryMethod(
                        () => new PermissionDependencyContext(_iocManager)
                        {
                            PermissionChecker = CreateMockPermissionChecker()
                        })
                );

            _iocManager.IocContainer.Register(
                Component.For<IFeatureDependencyContext, FeatureDependencyContext>()
                    .UsingFactoryMethod(
                        () => new FeatureDependencyContext(_iocManager, Substitute.For<IFeatureChecker>()))
                );

            //Create user navigation manager to test
            UserNavigationManager = new UserNavigationManager(NavigationManager, Substitute.For<ILocalizationContext>(), _iocManager);
        }

        private static IPermissionChecker CreateMockPermissionChecker()
        {
            var permissionChecker = Substitute.For<IPermissionChecker>();
            permissionChecker.IsGrantedAsync(new UserIdentifier(1, 1), "Geev.Zero.UserManagement").Returns(Task.FromResult(true));
            permissionChecker.IsGrantedAsync(new UserIdentifier(1, 1), "Geev.Zero.RoleManagement").Returns(Task.FromResult(false));
            return permissionChecker;
        }

        public class MyNavigationProvider1 : NavigationProvider
        {
            public override void SetNavigation(INavigationProviderContext context)
            {
                context.Manager.MainMenu.AddItem(
                    new MenuItemDefinition(
                        "Geev.Zero.Administration",
                        new FixedLocalizableString("Administration"),
                        "fa fa-asterisk",
                        requiresAuthentication: true
                        ).AddItem(
                            new MenuItemDefinition(
                                "Geev.Zero.Administration.User",
                                new FixedLocalizableString("User management"),
                                "fa fa-users",
                                "#/admin/users",
                                permissionDependency: new SimplePermissionDependency("Geev.Zero.UserManagement"),
                                customData: "A simple test data"
                                )
                        ).AddItem(
                            new MenuItemDefinition(
                                "Geev.Zero.Administration.Role",
                                new FixedLocalizableString("Role management"),
                                "fa fa-star-o",
                                "#/admin/roles",
                                permissionDependency: new SimplePermissionDependency("Geev.Zero.RoleManagement")
                                )
                        )
                    );
            }
        }

        public class MyNavigationProvider2 : NavigationProvider
        {
            public override void SetNavigation(INavigationProviderContext context)
            {
                var adminMenu = context.Manager.MainMenu.GetItemByName("Geev.Zero.Administration");
                adminMenu.AddItem(
                    new MenuItemDefinition(
                        "Geev.Zero.Administration.Setting",
                        new FixedLocalizableString("Setting management"),
                        icon: "fa fa-cog",
                        url: "#/admin/settings",
                        customData: new MyCustomDataClass { Data1 = 42, Data2 = "FortyTwo" }
                        )
                    );
            }
        }

        public class MyCustomDataClass
        {
            public int Data1 { get; set; }

            public string Data2 { get; set; }
        }
    }
}