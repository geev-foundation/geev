using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015123013)]
    public class _20151230_13_Revise_Users : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Column("Name").OnTable("GeevUsers").AsString(32).NotNullable();
            Alter.Column("Surname").OnTable("GeevUsers").AsString(32).NotNullable();
            Alter.Column("EmailAddress").OnTable("GeevUsers").AsString(256).NotNullable();
            Alter.Column("EmailConfirmationCode").OnTable("GeevUsers").AsString(128).Nullable();
            Alter.Column("PasswordResetCode").OnTable("GeevUsers").AsString(328).Nullable();
            Alter.Column("Password").OnTable("GeevUsers").AsString(128).NotNullable();

            Alter.Table("GeevUsers")
                .AddColumn("AuthenticationSource").AsString(64).Nullable();

            Create.Index("IX_IsDeleted_TenantId_EmailAddress")
                .OnTable("GeevUsers")
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("TenantId").Ascending()
                .OnColumn("EmailAddress").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_IsDeleted_TenantId_UserName")
                .OnTable("GeevUsers")
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("TenantId").Ascending()
                .OnColumn("UserName").Ascending()
                .WithOptions().NonClustered();
        }
    }
}