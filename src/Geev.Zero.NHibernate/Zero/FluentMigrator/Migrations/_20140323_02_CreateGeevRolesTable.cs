using Geev.FluentMigrator.Extensions;
using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2014032303)]
    public class _20140323_03_CreateGeevRolesTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("GeevRoles")
                .WithIdAsInt32()
                .WithColumn("Name").AsString(50).NotNullable()
                .WithColumn("DisplayName").AsString(100).NotNullable()
                .WithAuditColumns();

            Insert.IntoTable("GeevRoles").Row(
                new
                    {
                        Name = "Admin",
                        DisplayName = "Admin"
                    }
                );
        }
    }
}
