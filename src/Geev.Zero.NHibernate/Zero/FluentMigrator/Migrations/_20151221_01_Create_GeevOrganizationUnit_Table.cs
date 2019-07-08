using Geev.FluentMigrator.Extensions;
using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015122101)]
    public class _20151221_01_Create_GeevOrganizationUnit_Table : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("GeevOrganizationUnits")
                .WithIdAsInt64()
                .WithTenantIdAsNullable()
                .WithColumn("ParentId").AsInt64().Nullable().ForeignKey("GeevOrganizationUnits", "Id")
                .WithColumn("Code").AsString(128).NotNullable()
                .WithColumn("DisplayName").AsString(128).NotNullable()
                .WithFullAuditColumns();

            Create.Index("IX_TenantId_Code")
                .OnTable("GeevOrganizationUnits")
                .OnColumn("TenantId").Ascending()
                .OnColumn("Code").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_ParentId_Code")
                .OnTable("GeevOrganizationUnits")
                .OnColumn("ParentId").Ascending()
                .OnColumn("Code").Ascending()
                .WithOptions().NonClustered();
        }
    }
}