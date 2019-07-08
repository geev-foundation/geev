using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015123010)]
    public class _20151230_10_Revise_GeevUserLogins : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Column("LoginProvider").OnTable("GeevUserLogins").AsString(128).NotNullable();
            Alter.Column("ProviderKey").OnTable("GeevUserLogins").AsString(256).NotNullable();

            Create.Index("IX_UserId_LoginProvider")
                .OnTable("GeevUserLogins")
                .OnColumn("UserId").Ascending()
                .OnColumn("LoginProvider").Ascending()
                .WithOptions().NonClustered();
        }
    }
}