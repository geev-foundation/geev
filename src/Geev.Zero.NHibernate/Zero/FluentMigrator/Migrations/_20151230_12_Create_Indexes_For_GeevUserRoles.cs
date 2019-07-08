using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015123012)]
    public class _20151230_12_Create_Indexes_For_GeevUserRoles : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Index("IX_RoleId")
                .OnTable("GeevUserRoles")
                .OnColumn("RoleId").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_UserId_RoleId")
                .OnTable("GeevUserRoles")
                .OnColumn("UserId").Ascending()
                .OnColumn("RoleId").Ascending()
                .WithOptions().NonClustered();
        }
    }
}