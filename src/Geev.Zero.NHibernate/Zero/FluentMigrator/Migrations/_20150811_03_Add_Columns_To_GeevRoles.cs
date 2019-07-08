using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015081103)]
    public class _20150811_03_Add_Columns_To_GeevRoles : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("GeevRoles")
                .AddColumn("IsStatic").AsBoolean().NotNullable().WithDefaultValue(false)
                .AddColumn("IsDefault").AsBoolean().NotNullable().WithDefaultValue(false)
                .AddColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                .AddColumn("DeleterUserId").AsInt64().Nullable().ForeignKey("GeevUsers", "Id")
                .AddColumn("DeletionTime").AsDateTime().Nullable();
        }
    }
}