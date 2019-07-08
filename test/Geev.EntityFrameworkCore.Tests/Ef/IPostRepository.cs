using System;
using Geev.Domain.Repositories;
using Geev.EntityFrameworkCore.Tests.Domain;

namespace Geev.EntityFrameworkCore.Tests.Ef
{
    public interface IPostRepository : IRepository<Post, Guid>
    {
    }
}