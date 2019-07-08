using Microsoft.EntityFrameworkCore;

namespace Geev.EntityFrameworkCore.Repositories
{
    public interface IRepositoryWithDbContext
    {
        DbContext GetDbContext();
    }
}