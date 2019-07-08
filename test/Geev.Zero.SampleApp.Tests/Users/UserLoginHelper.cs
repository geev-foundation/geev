using System.Linq;
using Geev.Zero.SampleApp.EntityFramework;
using Geev.Zero.SampleApp.MultiTenancy;
using Geev.Zero.SampleApp.Users;

namespace Geev.Zero.SampleApp.Tests.Users
{
    public class UserLoginHelper
    {
        public static void CreateTestUsers(AppDbContext context)
        {
            var defaultTenant = context.Tenants.Single(t => t.TenancyName == Tenant.DefaultTenantName);

            CreateTestUsers(context,
                new User
                {
                    UserName = "userOwner",
                    Name = "Owner",
                    Surname = "One",
                    EmailAddress = "owner@aspnetboilerplate.com",
                    IsEmailConfirmed = true,
                    Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                });


            CreateTestUsers(context,
                new User
                {
                    TenantId = defaultTenant.Id, //A user of tenant1
                    UserName = "user1",
                    Name = "User",
                    Surname = "One",
                    EmailAddress = "user-one@aspnetboilerplate.com",
                    IsEmailConfirmed = false,
                    Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                });
        }

        private static void CreateTestUsers(AppDbContext context, User user)
        {
            user.SetNormalizedNames();

            context.Users.Add(user);
        }
    }
}