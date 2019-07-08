using System.Threading.Tasks;
using Geev.Configuration.Startup;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.Runtime.Security;
using Geev.ZeroCore.SampleApp.Core;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Xunit;

namespace Geev.IdentityServer4
{
    public class DependencyInjection_Tests : GeevZeroIdentityServerTestBase
    {
        [Fact]
        public void Should_Inject_GeevPersistedGrantStore()
        {
            Resolve<GeevPersistedGrantStore>();
        }

        [Fact]
        public async Task Should_Inject_GeevUserClaimsPrincipalFactory()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = true;
            GeevSession.TenantId = 1;

            var repository = Resolve<IRepository<User, long>>();

            var userToAdd = User.CreateTenantAdminUser(GeevSession.TenantId.Value, "admin@test.com");
            userToAdd.Password = "123qwe";
            var userId = await repository.InsertAndGetIdAsync(userToAdd);

            using (var uow = Resolve<IUnitOfWorkManager>().Begin())
            {
                //Arrange
                var user = repository.FirstOrDefault(userId);
                user.ShouldNotBeNull();

                var principalFactory = Resolve<IUserClaimsPrincipalFactory<User>>();

                //Act
                var identity = (await principalFactory.CreateAsync(user)).Identity;

                //Assert
                identity.GetTenantId().ShouldBe(GeevSession.TenantId);
                identity.GetUserId().ShouldBe(user.Id);

                await uow.CompleteAsync();
            }
        }
    }
}
