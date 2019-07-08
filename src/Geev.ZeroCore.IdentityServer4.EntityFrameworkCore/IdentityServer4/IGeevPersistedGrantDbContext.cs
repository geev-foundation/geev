using Microsoft.EntityFrameworkCore;

namespace Geev.IdentityServer4
{
    public interface IGeevPersistedGrantDbContext
    {
        DbSet<PersistedGrantEntity> PersistedGrants { get; set; }
    }
}