using Geev.FluentMigrator.Extensions;
using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2016052401)]
    public class _20160524_01_Fix_Inconsistent_Data_Type : Migration
    {
        public override void Up()
        {
            this.Delete.PrimaryKey("PK_GeevBackgroundJobs")
               .FromTable("GeevBackgroundJobs");
            this.Alter.Column("Id").OnTable("GeevBackgroundJobs")
                .AsInt64().NotNullable();
            this.Create.PrimaryKey("PK_GeevBackgroundJobs")
                .OnTable("GeevBackgroundJobs")
                .Column("Id");

            this.Delete.Index("IX_RoleId_Name")
                .OnTable("GeevPermissions");
            this.Delete.Index("IX_UserId_Name")
                .OnTable("GeevPermissions");
            this.Alter.Column("Name")
                .OnTable("GeevPermissions")
                .AsString(128)
                .NotNullable();

            this.Create.Index("IX_RoleId_Name")
                .OnTable("GeevPermissions")
                .OnColumn("RoleId").Ascending()
                .OnColumn("Name").Ascending()
                .WithOptions().NonClustered();
            this.Create.Index("IX_UserId_Name")
                .OnTable("GeevPermissions")
                .OnColumn("UserId").Ascending()
                .OnColumn("Name").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            this.Delete.Index("IX_RoleId_Name")
                .OnTable("GeevPermissions");
            this.Delete.Index("IX_UserId_Name")
                .OnTable("GeevPermissions");
            this.Alter.Column("Name")
                .OnTable("GeevPermissions")
                .AsAnsiString(128)
                .NotNullable();
            this.Create.Index("IX_RoleId_Name")
                .OnTable("GeevPermissions")
                .OnColumn("RoleId").Ascending()
                .OnColumn("Name").Ascending()
                .WithOptions().NonClustered();
            this.Create.Index("IX_UserId_Name")
                .OnTable("GeevPermissions")
                .OnColumn("UserId").Ascending()
                .OnColumn("Name").Ascending()
                .WithOptions().NonClustered();

            this.Delete.PrimaryKey("PK_GeevBackgroundJobs")
                .FromTable("GeevBackgroundJobs");
            this.Alter.Column("Id").OnTable("GeevBackgroundJobs")
                .AsInt32().NotNullable().PrimaryKey("PK_GeevBackgroundJobs");
            this.Create.PrimaryKey("PK_GeevBackgroundJobs")
                .OnTable("GeevBackgroundJobs")
                .Column("Id");
        }
    }
}
