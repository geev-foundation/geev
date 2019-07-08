using Geev.FluentMigrator.Extensions;
using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015123001)]
    public class _20151230_01_Create_Editions_And_Features_Tables : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("GeevEditions")
                .WithIdAsInt32()
                .WithColumn("Name").AsString(32).NotNullable().Indexed("IX_Name")
                .WithColumn("DisplayName").AsString(64).NotNullable()
                .WithFullAuditColumns();

            Create.Table("GeevFeatures")
                .WithIdAsInt64()
                .WithColumn("Name").AsString(128).NotNullable()
                .WithColumn("Value").AsString(2000).NotNullable()
                .WithColumn("EditionId").AsInt32().Nullable().ForeignKey("GeevEditions", "Id").Indexed("IX_EditionId")
                .WithTenantIdAsNullable()
                .WithColumn("Discriminator").AsString(128).NotNullable()
                .WithCreationAuditColumns();

            Create.Index("IX_TenantId_Name")
                .OnTable("GeevFeatures")
                .OnColumn("TenantId").Ascending()
                .OnColumn("Name").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_Discriminator_TenantId_Name")
                .OnTable("GeevFeatures")
                .OnColumn("Discriminator").Ascending()
                .OnColumn("TenantId").Ascending()
                .OnColumn("Name").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_Discriminator_EditionId_Name")
                .OnTable("GeevFeatures")
                .OnColumn("Discriminator").Ascending()
                .OnColumn("EditionId").Ascending()
                .OnColumn("Name").Ascending()
                .WithOptions().NonClustered();
        }
    }
}