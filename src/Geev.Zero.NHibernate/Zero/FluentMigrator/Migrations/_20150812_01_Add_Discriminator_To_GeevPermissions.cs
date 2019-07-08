using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015081201)]
    public class _20150812_01_Add_Discriminator_To_GeevPermissions : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("GeevPermissions")
                .AddColumn("Discriminator").AsString(128).NotNullable();
        }
    }
}