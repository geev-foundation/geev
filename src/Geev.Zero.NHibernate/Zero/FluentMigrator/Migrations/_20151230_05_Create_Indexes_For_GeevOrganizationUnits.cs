using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015123005)]
    public class _20151230_05_Create_Indexes_For_GeevOrganizationUnits : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Index("IX_TenantId_ParentId")
                .OnTable("GeevOrganizationUnits")
                .OnColumn("TenantId").Ascending()
                .OnColumn("ParentId").Ascending()
                .WithOptions().NonClustered();
        }
    }
}
