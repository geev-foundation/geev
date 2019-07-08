namespace Geev.MultiTenancy
{
    public interface IGeevZeroDbMigrator
    {
        void CreateOrMigrateForHost();

        void CreateOrMigrateForTenant(GeevTenantBase tenant);
    }
}
