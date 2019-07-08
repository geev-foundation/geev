namespace Geev.MultiTenancy
{
    public interface ITenantResolver
    {
        int? ResolveTenantId();
    }
}