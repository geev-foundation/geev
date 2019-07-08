using System;
using System.Net;
using System.Threading.Tasks;
using Geev.AspNetCore.App.Controllers;
using Geev.Web.Models;
using Shouldly;
using Xunit;

namespace Geev.AspNetCore.Tests
{
    public class AuthTestController_Tests : AppTestBase
    {
        [Fact]
        public async Task Can_Call_NonAuthorized_Actions_Anonymously()
        {
            // Act
            var response = await GetResponseAsStringAsync(
                GetUrl<AuthTestController>(
                    nameof(AuthTestController.NonAuthorizedAction)
                )
            );

            // Assert
            response.ShouldBe("public content");

            // Act
            response = await GetResponseAsStringAsync(
                GetUrl<AuthTest2Controller>(
                    nameof(AuthTest2Controller.NonAuthorizedAction)
                )
            );

            // Assert
            response.ShouldBe("public content 2");
        }

        [Fact]
        public async Task Can_Not_Call_Authorized_Actions_Anonymously()
        {
            // Act & Assert
            await Assert.ThrowsAnyAsync<Exception>(async () =>
            {
                await GetResponseAsStringAsync(
                    GetUrl<AuthTestController>(
                        nameof(AuthTestController.AuthorizedAction)
                    )
                );
            });

            // Act & Assert
            await Assert.ThrowsAnyAsync<Exception>(async () =>
            {
                await GetResponseAsStringAsync(
                    GetUrl<AuthTestController>(
                        nameof(AuthTestController.GeevMvcAuthorizedAction)
                    )
                );
            });

            //Act
            var response = await GetResponseAsObjectAsync<AjaxResponse>(
                    GetUrl<AuthTestController>(
                        nameof(AuthTestController.GeevMvcAuthorizedActionReturnsObject)
                    ),
                    HttpStatusCode.Unauthorized
                );

            //Assert
            response.Success.ShouldBeFalse();
            response.Result.ShouldBe(null);
            response.Error.ShouldNotBeNull();
            response.Error.Message.ShouldNotBeNull();
            response.UnAuthorizedRequest.ShouldBeTrue();

            // Act & Assert
            await Assert.ThrowsAnyAsync<Exception>(async () =>
            {
                await GetResponseAsStringAsync(
                    GetUrl<AuthTest2Controller>(
                        nameof(AuthTest2Controller.AuthorizedAction)
                    )
                );
            });
        }
    }
}