using System.Linq;
using System.Threading.Tasks;
using Geev.Application.Navigation;
using Shouldly;
using Xunit;

namespace Geev.Tests.Application.Navigation
{
    public class Menu_Tests : TestBaseWithLocalIocManager
    {
        [Fact]
        public async Task Test_Menu_System()
        {
            var testCase = new NavigationTestCase();

            //Check created menu definitions
            var mainMenuDefinition = testCase.NavigationManager.MainMenu;
            mainMenuDefinition.Items.Count.ShouldBe(1);

            var adminMenuItemDefinition = mainMenuDefinition.GetItemByNameOrNull("Geev.Zero.Administration");
            adminMenuItemDefinition.ShouldNotBe(null);
            adminMenuItemDefinition.Items.Count.ShouldBe(3);
            
            //Check user menus
            var userMenu = await testCase.UserNavigationManager.GetMenuAsync(mainMenuDefinition.Name, new UserIdentifier(1, 1));
            userMenu.Items.Count.ShouldBe(1);

            var userAdminMenu = userMenu.Items.FirstOrDefault(i => i.Name == "Geev.Zero.Administration");
            userAdminMenu.ShouldNotBe(null);

            userAdminMenu.Items.FirstOrDefault(i => i.Name == "Geev.Zero.Administration.User").ShouldNotBe(null);
            userAdminMenu.Items.FirstOrDefault(i => i.Name == "Geev.Zero.Administration.Role").ShouldBe(null);
            userAdminMenu.Items.FirstOrDefault(i => i.Name == "Geev.Zero.Administration.Setting").ShouldNotBe(null);

            mainMenuDefinition.RemoveItem(mainMenuDefinition.Items.FirstOrDefault()?.Name);
            mainMenuDefinition.Items.Count.ShouldBe(0);
        }
    }
}
