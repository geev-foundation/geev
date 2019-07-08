using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015123006)]
    public class _20151230_06_Create_Indexes_For_GeevPermissions : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Index("IX_RoleId_Name")
                .OnTable("GeevPermissions")
                .OnColumn("RoleId").Ascending()
                .OnColumn("Name").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_UserId_Name")
                .OnTable("GeevPermissions")
                .OnColumn("UserId").Ascending()
                .OnColumn("Name").Ascending()
                .WithOptions().NonClustered();
        }
    }
}