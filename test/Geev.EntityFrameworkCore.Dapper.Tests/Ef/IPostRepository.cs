using System;

using Geev.Domain.Repositories;
using Geev.EntityFrameworkCore.Dapper.Tests.Domain;

namespace Geev.EntityFrameworkCore.Dapper.Tests.Ef
{
    public interface IPostRepository : IRepository<Post, Guid>
    {
    }
}