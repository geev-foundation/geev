using Geev.EntityFrameworkCore;
using GeevAspNetCoreDemo.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace GeevAspNetCoreDemo.Db
{
    public class MyDbContext : GeevDbContext
    {
        public DbSet<Product> Products { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product("Test product", 100)
            {
                Id = 1
            });
        }
    }
}