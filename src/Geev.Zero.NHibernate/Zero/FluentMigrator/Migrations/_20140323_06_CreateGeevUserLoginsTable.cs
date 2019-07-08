using Geev.FluentMigrator.Extensions;
using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2014032306)]
    public class _20140323_06_CreateGeevUserLoginsTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("GeevUserLogins")
                .WithIdAsInt64()
                .WithUserId()
                .WithColumn("LoginProvider").AsAnsiString(100).NotNullable()
                .WithColumn("ProviderKey").AsAnsiString(100).NotNullable();
        }
    }
}