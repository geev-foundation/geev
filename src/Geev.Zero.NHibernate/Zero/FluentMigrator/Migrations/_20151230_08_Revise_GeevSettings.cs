using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015123008)]
    public class _20151230_08_Revise_GeevSettings : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Column("Name").OnTable("GeevSettings").AsString(256).NotNullable();
            Alter.Column("Value").OnTable("GeevSettings").AsString(2000).Nullable();

            Create.Index("IX_UserId_Name")
                .OnTable("GeevSettings")
                .OnColumn("UserId").Ascending()
                .OnColumn("Name").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_TenantId_Name")
                .OnTable("GeevSettings")
                .OnColumn("TenantId").Ascending()
                .OnColumn("Name").Ascending()
                .WithOptions().NonClustered();
        }
    }
}