using Geev.EntityFrameworkCore.Tests.Domain;
using Microsoft.EntityFrameworkCore;

namespace Geev.EntityFrameworkCore.Tests.Ef
{
    public class BloggingDbContext : GeevDbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<BlogCategory> BlogCategories { get; set; }

        public DbSet<SubBlogCategory> SubBlogCategories { get; set; }

        public BloggingDbContext(DbContextOptions<BloggingDbContext> options)
            : base(options)
        {
            
        }
    }
}
