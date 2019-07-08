using Geev.EntityFrameworkCore.Dapper.Tests.Domain;

using Microsoft.EntityFrameworkCore;

namespace Geev.EntityFrameworkCore.Dapper.Tests.Ef
{
    public class BloggingDbContext : GeevDbContext
    {
        public BloggingDbContext(DbContextOptions<BloggingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}
