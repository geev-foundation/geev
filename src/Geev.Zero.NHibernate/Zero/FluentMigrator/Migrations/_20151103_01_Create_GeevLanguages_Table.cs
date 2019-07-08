using Geev.FluentMigrator.Extensions;
using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015110301)]
    public class _20151103_01_Create_GeevLanguages_Table : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("GeevLanguages")
                .WithIdAsInt32()
                .WithTenantIdAsNullable()
                .WithColumn("Name").AsString(10).NotNullable()
                .WithColumn("DisplayName").AsString(64).NotNullable()
                .WithColumn("Icon").AsString(128).Nullable()
                .WithFullAuditColumns();
        }
    }
}