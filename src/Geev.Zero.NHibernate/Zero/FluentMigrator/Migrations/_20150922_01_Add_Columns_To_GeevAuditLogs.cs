using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015092201)]
    public class _20150922_01_Add_Columns_To_GeevAuditLogs : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("GeevAuditLogs")
                .AddColumn("ImpersonatorUserId").AsInt64().Nullable()
                .AddColumn("ImpersonatorTenantId").AsInt32().Nullable()
                .AddColumn("CustomData").AsString(2000).Nullable();
        }
    }
}