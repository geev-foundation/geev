using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2014082901)]
    public class _20140829_01_Add_Columns_To_GeevUsers_Table : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("GeevUsers")
                .AddCreationAuditColumns()
                .AddColumn("LastLoginTime").AsDateTime().Nullable();
        }
    }
}