using Geev.MultiTenancy;

namespace Geev.Zero.EntityFrameworkCore
{
    public interface IMultiTenantSeed
    {
        GeevTenantBase Tenant { get; set; }
    }
}