using Geev.FluentMigrator.Extensions;
using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2014032305)]
    public class _20140323_05_CreatePermissionsTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("GeevPermissions")
                .WithIdAsInt64()
                .WithColumn("RoleId").AsInt32().Nullable().ForeignKey("GeevRoles", "Id")
                .WithNullableUserId()
                .WithColumn("Name").AsAnsiString(128).NotNullable()
                .WithColumn("IsGranted").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithCreationAuditColumns();
        }
    }
}