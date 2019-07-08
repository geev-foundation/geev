using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Geev.Authorization.Users;
using Geev.Domain.Uow;
using Geev.ZeroCore.SampleApp.Core;
using Shouldly;
using Xunit;

namespace Geev.Zero.Users
{
    public class UserManager_AddToRole_Tests : GeevZeroTestBase
    {
        [Fact]
        public async Task AddToRoleAsync_Test()
        {
            using (var uow = Resolve<IUnitOfWorkManager>().Begin())
            {
                var user = new User
                {
                    TenantId = GeevSession.TenantId,
                    UserName = "user1",
                    Name = "John",
                    Surname = "Doe",
                    EmailAddress = "user1@aspnetboilerplate.com",
                    IsEmailConfirmed = true,
                    Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==", //123qwe
                    // IsLockoutEnabled = isLockoutEnabled
                };

                var userManager = LocalIocManager.Resolve<UserManager>();
                await userManager.CreateAsync(user);
                await userManager.AddToRoleAsync(user, "ADMIN");

                user.Roles.Count.ShouldBe(1);

                await uow.CompleteAsync();
            }
        }
    }
}
