using System.Linq;
using Geev.Zero.SampleApp.EntityFramework;
using Geev.Zero.SampleApp.MultiTenancy;
using Geev.Zero.SampleApp.Users;
using Microsoft.AspNet.Identity;

namespace Geev.Zero.SampleApp.Tests.TestDatas
{
    public class InitialUsersBuilder
    {
        private readonly AppDbContext _context;

        public InitialUsersBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            CreateUsers();
        }

        private void CreateUsers()
        {
            var defaultTenant = _context.Tenants.Single(t => t.TenancyName == Tenant.DefaultTenantName);

            CreateUser(new User
            {
                TenantId = defaultTenant.Id,
                Name = "System",
                Surname = "Administrator",
                UserName = User.AdminUserName,
                Password = new PasswordHasher().HashPassword("123qwe"),
                EmailAddress = "admin@aspnetboilerplate.com"
            });

            CreateUser(
                new User
                {
                    TenantId = defaultTenant.Id,
                    Name = "System",
                    Surname = "Manager",
                    UserName = "manager",
                    Password = new PasswordHasher().HashPassword("123qwe"),
                    EmailAddress = "manager@aspnetboilerplate.com"
                });
        }

        private void CreateUser(User user)
        {
            user.SetNormalizedNames();
            _context.Users.Add(user);
        }
    }
}