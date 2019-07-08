using Geev.FluentMigrator.Extensions;
using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2014032304)]
    public class _20140323_04_CreateGeevUserRolesTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("GeevUserRoles")
                .WithIdAsInt64()
                .WithUserId()
                .WithColumn("RoleId").AsInt32().NotNullable().ForeignKey("GeevRoles", "Id")
                .WithCreationAuditColumns();

            Insert.IntoTable("GeevUserRoles").Row(
                new
                    {
                        UserId = 1,
                        RoleId = 1
                    }
                );
        }
    }
}