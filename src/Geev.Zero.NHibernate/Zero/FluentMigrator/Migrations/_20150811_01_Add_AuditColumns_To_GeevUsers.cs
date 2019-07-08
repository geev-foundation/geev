using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015081101)]
    public class _20150811_01_Add_AuditColumns_To_GeevUsers : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("GeevUsers")
                .AddColumn("LastModificationTime").AsDateTime().Nullable()
                .AddColumn("LastModifierUserId").AsInt64().Nullable().ForeignKey("GeevUsers", "Id");
        }
    }
}