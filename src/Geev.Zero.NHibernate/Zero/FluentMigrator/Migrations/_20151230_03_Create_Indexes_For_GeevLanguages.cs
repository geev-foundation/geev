using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015123003)]
    public class _20151230_03_Create_Indexes_For_GeevLanguages : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Index("IX_TenantId_Name")
                .OnTable("GeevLanguages")
                .OnColumn("TenantId").Ascending()
                .OnColumn("Name").Ascending()
                .WithOptions().NonClustered();
        }
    }
}