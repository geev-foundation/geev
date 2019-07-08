using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2015123009)]
    public class _20151230_09_Revise_GeevTenants : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Column("TenancyName").OnTable("GeevTenants").AsString(64).NotNullable();

            Alter.Table("GeevTenants")
                .AddColumn("EditionId").AsInt32().Nullable().ForeignKey("GeevEditions", "Id")
                .AddCreatorUserIdColumn()
                .AddModificationAuditColumns()
                .AddDeletionAuditColumns();

            Create.Index("IX_EditionId")
                .OnTable("GeevTenants")
                .OnColumn("EditionId").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_IsDeleted_TenancyName")
                .OnTable("GeevTenants")
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("TenancyName").Ascending()
                .WithOptions().NonClustered();
        }
    }
}