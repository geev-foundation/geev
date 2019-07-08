using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using Geev.Zero.EntityFramework;
using Geev.Zero.SampleApp.BookStore;
using Geev.Zero.SampleApp.EntityHistory;
using Geev.Zero.SampleApp.MultiTenancy;
using Geev.Zero.SampleApp.Roles;
using Geev.Zero.SampleApp.Users;

namespace Geev.Zero.SampleApp.EntityFramework
{
    public class AppDbContext : GeevZeroDbContext<Tenant, Role, User>
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Store> Stores { get; set; }

        public AppDbContext(DbConnection existingConnection)
            : base(existingConnection, true)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>().HasRequired(e => e.Post).WithMany(e => e.Comments);

            modelBuilder.Entity<Book>().ToTable("Books");
            modelBuilder.Entity<Book>().Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Store>().Property(e => e.Id).HasColumnName("StoreId");
        }
    }
}