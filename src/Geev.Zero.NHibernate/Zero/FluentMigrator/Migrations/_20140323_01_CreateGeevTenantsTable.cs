using Geev.FluentMigrator.Extensions;
using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2014032301)]
    public class _20140323_01_CreateGeevTenantsTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("GeevTenants")
                .WithIdAsInt32()
                .WithColumn("TenancyName").AsString(32).NotNullable()
                .WithColumn("Name").AsString(128).NotNullable()
                .WithCreationTimeColumn();

            Create.Index("GeevTenants_TenancyName")
                .OnTable("GeevTenants")
                .OnColumn("TenancyName").Ascending()
                .WithOptions().Unique()
                .WithOptions().NonClustered();

            //Default tenant
            Insert.IntoTable("GeevTenants").Row(
                new
                {
                    TenancyName = "Default", //Reserved TenancyName
                    Name = "Default"
                });
        }
    }
}