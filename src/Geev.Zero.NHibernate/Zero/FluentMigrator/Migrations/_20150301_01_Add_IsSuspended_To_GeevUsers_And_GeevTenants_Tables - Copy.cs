using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015030101)]
    public class _20150301_01_Add_IsActive_To_GeevUsers_And_GeevTenants_Tables : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("GeevUsers").AddColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
            Alter.Table("GeevTenants").AddColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
        }
    }
}