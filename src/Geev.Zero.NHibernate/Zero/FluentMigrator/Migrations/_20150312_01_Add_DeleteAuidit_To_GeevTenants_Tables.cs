using Geev.FluentMigrator.Extensions;
using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015031201)]
    public class _20150312_01_Add_DeleteAuidit_To_GeevTenants_Tables : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("GeevUsers")
                .AddIsDeletedColumn()
                .AddColumn("DeleterUserId").AsInt64().Nullable().ForeignKey("GeevUsers", "Id")
                .AddColumn("DeletionTime").AsDateTime().Nullable();
        }
    }
}