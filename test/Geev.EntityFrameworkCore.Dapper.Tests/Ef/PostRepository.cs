using System;

using Geev.EntityFrameworkCore.Dapper.Tests.Domain;
using Geev.EntityFrameworkCore.Repositories;

namespace Geev.EntityFrameworkCore.Dapper.Tests.Ef
{
    public class PostRepository : EfCoreRepositoryBase<BloggingDbContext, Post, Guid>, IPostRepository
    {
        public PostRepository(IDbContextProvider<BloggingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public override int Count()
        {
            throw new Exception("can not get count of posts");
        }
    }
}
