﻿using Geev.FluentMigrator.Extensions;
using FluentMigrator;

namespace Geev.Zero.FluentMigrator.Migrations
{
    [Migration(2014032302)]
    public class _20140323_02_CreateGeevUsersTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("GeevUsers")
                .WithIdAsInt64()
                .WithTenantIdAsNullable()
                .WithColumn("UserName").AsString(32).NotNullable()
                .WithColumn("Name").AsString(30).NotNullable()
                .WithColumn("Surname").AsString(30).NotNullable()
                .WithColumn("EmailAddress").AsString(100).NotNullable()
                .WithColumn("IsEmailConfirmed").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("EmailConfirmationCode").AsString(16).Nullable()
                .WithColumn("PasswordResetCode").AsString(32).Nullable()
                .WithColumn("Password").AsString(100).NotNullable();

            Create.Index("GeevUsers_TenantId_UserName")
                .OnTable("GeevUsers")
                .OnColumn("TenantId").Ascending()
                .OnColumn("UserName").Ascending()
                .WithOptions().Unique()
                .WithOptions().NonClustered();
            
            Create.Index("GeevUsers_TenantId_EmailAddress")
                .OnTable("GeevUsers")
                .OnColumn("TenantId").Ascending()
                .OnColumn("EmailAddress").Ascending()
                .WithOptions().Unique()
                .WithOptions().NonClustered();

            //User for system admin.
            Insert.IntoTable("GeevUsers").Row(
                new
                {
                    UserName = "admin",
                    Name = "System",
                    Surname = "Administrator",
                    EmailAddress = "admin@aspnetboilerplate.com",
                    IsEmailConfirmed = true,
                    Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                });

            //User for default tenant's admin.
            Insert.IntoTable("GeevUsers").Row(
                new
                {
                    TenantId = 1,
                    UserName = "admin",
                    Name = "System",
                    Surname = "Administrator",
                    EmailAddress = "admin@aspnetboilerplate.com",
                    IsEmailConfirmed = true,
                    Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                });
        }
    }
}
