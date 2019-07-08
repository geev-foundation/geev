using Geev.Zero.EntityFrameworkCore;
using Geev.Zero.SampleApp.MultiTenancy;
using Geev.Zero.SampleApp.Roles;
using Geev.Zero.SampleApp.Users;
using Microsoft.EntityFrameworkCore;

namespace Geev.Zero.SampleApp.EntityFrameworkCore
{
    public class AppDbContext : GeevZeroDbContext<Tenant, Role, User, AppDbContext>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {

        }
    }
}