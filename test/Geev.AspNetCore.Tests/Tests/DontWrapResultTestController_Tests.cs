using System.Threading.Tasks;
using Geev.AspNetCore.App.Controllers;
using Shouldly;
using Xunit;

namespace Geev.AspNetCore.Tests
{
    public class DontWrapResultTestController_Tests : AppTestBase
    {
        [Fact]
        public async Task DontWrapResultTestControllerTests_Get_Test()
        {
            // Act
            var response = await GetResponseAsStringAsync(
                GetUrl<DontWrapResultTestController>(
                    nameof(DontWrapResultTestController.Get)
                )
            );

            // Assert
            response.ShouldBe("42");
        }

        [Fact]
        public async Task DontWrapResultTestControllerTests_GetBase_Test()
        {
            // Act
            var response = await GetResponseAsStringAsync(
                GetUrl<DontWrapResultTestController>(
                    nameof(DontWrapResultTestController.GetBase)
                )
            );

            // Assert
            response.ShouldBe("42");
        }
    }
}