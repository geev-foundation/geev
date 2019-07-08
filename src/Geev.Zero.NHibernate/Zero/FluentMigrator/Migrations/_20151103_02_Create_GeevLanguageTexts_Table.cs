using Geev.FluentMigrator.Extensions;
using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015110302)]
    public class _20151103_02_Create_GeevLanguageTexts_Table : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("GeevLanguageTexts")
                .WithIdAsInt64()
                .WithTenantIdAsNullable()
                .WithColumn("LanguageName").AsString(10).NotNullable()
                .WithColumn("Source").AsString(128).NotNullable()
                .WithColumn("Key").AsString(256).NotNullable()
                .WithColumn("Value").AsString(64 * 1024 * 1024).NotNullable() //64KB
                .WithAuditColumns();
        }
    }
}