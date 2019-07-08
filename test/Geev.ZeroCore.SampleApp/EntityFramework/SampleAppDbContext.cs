using System.ComponentModel.DataAnnotations.Schema;
using Geev.IdentityServer4;
using Geev.Zero.EntityFrameworkCore;
using Geev.ZeroCore.SampleApp.Core;
using Geev.ZeroCore.SampleApp.Core.BookStore;
using Geev.ZeroCore.SampleApp.Core.EntityHistory;
using Geev.ZeroCore.SampleApp.Core.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Geev.ZeroCore.SampleApp.EntityFramework
{
    //TODO: Re-enable when IdentityServer ready
    public class SampleAppDbContext : GeevZeroDbContext<Tenant, Role, User, SampleAppDbContext>, IGeevPersistedGrantDbContext
    {
        public DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductTranslation> ProductTranslations { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderTranslation> OrderTranslations { get; set; }

        public SampleAppDbContext(DbContextOptions<SampleAppDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePersistedGrantEntity();

            modelBuilder.Entity<Blog>().OwnsOne(x => x.More);

            modelBuilder.Entity<Book>().ToTable("Books");
            modelBuilder.Entity<Book>().Property(e => e.Id).ValueGeneratedNever();

            modelBuilder.Entity<Store>().Property(e => e.Id).HasColumnName("StoreId");
        }
    }
}
