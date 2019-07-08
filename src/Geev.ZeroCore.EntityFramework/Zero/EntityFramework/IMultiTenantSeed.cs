using Geev.MultiTenancy;

namespace Geev.Zero.EntityFramework
{
    public interface IMultiTenantSeed
    {
        GeevTenantBase Tenant { get; set; }
    }
}