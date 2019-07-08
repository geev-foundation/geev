using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2014083001)]
    public class _20140830_01_Add_Columns_To_GeevRoles_Table : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("GeevRoles")
                .AddTenantIdColumnAsNullable();
        }
    }
}