using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015123007)]
    public class _20151230_07_Revise_GeevRoles : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Column("Name").OnTable("GeevRoles").AsString(32).NotNullable();
            Alter.Column("DisplayName").OnTable("GeevRoles").AsString(64).NotNullable();

            Create.Index("IX_IsDeleted_TenantId_Name")
                .OnTable("GeevRoles")
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("TenantId").Ascending()
                .OnColumn("Name").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_TenantId_Name")
                .OnTable("GeevRoles")
                .OnColumn("TenantId").Ascending()
                .OnColumn("Name").Ascending()
                .WithOptions().NonClustered();
        }
    }
}