using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015123011)]
    public class _20151230_11_Create_Indexes_For_GeevUserOrganizationUnits : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Index("IX_OrganizationUnitId")
                .OnTable("GeevUserOrganizationUnits")
                .OnColumn("OrganizationUnitId").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_UserId")
                .OnTable("GeevUserOrganizationUnits")
                .OnColumn("UserId").Ascending()
                .WithOptions().NonClustered();
        }
    }
}