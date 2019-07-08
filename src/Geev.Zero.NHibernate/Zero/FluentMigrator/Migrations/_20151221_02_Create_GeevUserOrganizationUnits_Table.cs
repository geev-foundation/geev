using Geev.FluentMigrator.Extensions;
using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015122102)]
    public class _20151221_02_Create_GeevUserOrganizationUnits_Table : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("GeevUserOrganizationUnits")
                .WithIdAsInt64()
                .WithTenantIdAsNullable()
                .WithUserId()
                .WithColumn("OrganizationUnitId").AsInt64().NotNullable().ForeignKey("GeevOrganizationUnits", "Id")
                .WithCreationAuditColumns();

            Create.Index("IX_TenantId_UserId")
                .OnTable("GeevUserOrganizationUnits")
                .OnColumn("TenantId").Ascending()
                .OnColumn("UserId").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_TenantId_OrganizationUnitId")
                .OnTable("GeevUserOrganizationUnits")
                .OnColumn("TenantId").Ascending()
                .OnColumn("OrganizationUnitId").Ascending()
                .WithOptions().NonClustered();
        }
    }
}