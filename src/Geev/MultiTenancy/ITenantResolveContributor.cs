namespace Geev.MultiTenancy
{
    public interface ITenantResolveContributor
    {
        int? ResolveTenantId();
    }
}