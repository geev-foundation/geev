using Geev.FluentMigrator.Extensions;
using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2014032307)]
    public class _20140323_07_CreateGeevSettingsTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("GeevSettings")
                .WithIdAsInt64()
                .WithTenantIdAsNullable()
                .WithNullableUserId()
                .WithColumn("Name").AsAnsiString(128).NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithAuditColumns();
        }
    }
}