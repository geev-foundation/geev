using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Geev.Zero.SampleApp.EntityFrameworkCore.Migrations
{
    public partial class Remove_Unique_Constraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeevTenants_GeevUsers_CreatorUserId1",
                table: "GeevTenants");

            migrationBuilder.DropForeignKey(
                name: "FK_GeevTenants_GeevUsers_DeleterUserId1",
                table: "GeevTenants");

            migrationBuilder.DropForeignKey(
                name: "FK_GeevTenants_GeevUsers_LastModifierUserId1",
                table: "GeevTenants");

            migrationBuilder.DropIndex(
                name: "IX_GeevUsers_CreatorUserId",
                table: "GeevUsers");

            migrationBuilder.DropIndex(
                name: "IX_GeevUsers_LastModifierUserId",
                table: "GeevUsers");

            migrationBuilder.RenameColumn(
                name: "LastModifierUserId1",
                table: "GeevTenants",
                newName: "UserId2");

            migrationBuilder.RenameColumn(
                name: "DeleterUserId1",
                table: "GeevTenants",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "CreatorUserId1",
                table: "GeevTenants",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GeevTenants_LastModifierUserId1",
                table: "GeevTenants",
                newName: "IX_GeevTenants_UserId2");

            migrationBuilder.RenameIndex(
                name: "IX_GeevTenants_DeleterUserId1",
                table: "GeevTenants",
                newName: "IX_GeevTenants_UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_GeevTenants_CreatorUserId1",
                table: "GeevTenants",
                newName: "IX_GeevTenants_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "EmailConfirmationCode",
                table: "GeevUsers",
                maxLength: 328,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "GeevUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLockoutEnabled",
                table: "GeevUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPhoneNumberConfirmed",
                table: "GeevUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTwoFactorEnabled",
                table: "GeevUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutEndDateUtc",
                table: "GeevUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "GeevUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "GeevUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GeevUserClaims",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeevUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeevUserClaims_GeevUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "GeevUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeevUsers_CreatorUserId",
                table: "GeevUsers",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GeevUsers_DeleterUserId",
                table: "GeevUsers",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GeevUsers_LastModifierUserId",
                table: "GeevUsers",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GeevUserClaims_UserId",
                table: "GeevUserClaims",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeevTenants_GeevUsers_UserId",
                table: "GeevTenants",
                column: "UserId",
                principalTable: "GeevUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeevTenants_GeevUsers_UserId1",
                table: "GeevTenants",
                column: "UserId1",
                principalTable: "GeevUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeevTenants_GeevUsers_UserId2",
                table: "GeevTenants",
                column: "UserId2",
                principalTable: "GeevUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeevUsers_GeevUsers_DeleterUserId",
                table: "GeevUsers",
                column: "DeleterUserId",
                principalTable: "GeevUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeevTenants_GeevUsers_UserId",
                table: "GeevTenants");

            migrationBuilder.DropForeignKey(
                name: "FK_GeevTenants_GeevUsers_UserId1",
                table: "GeevTenants");

            migrationBuilder.DropForeignKey(
                name: "FK_GeevTenants_GeevUsers_UserId2",
                table: "GeevTenants");

            migrationBuilder.DropForeignKey(
                name: "FK_GeevUsers_GeevUsers_DeleterUserId",
                table: "GeevUsers");

            migrationBuilder.DropTable(
                name: "GeevUserClaims");

            migrationBuilder.DropIndex(
                name: "IX_GeevUsers_CreatorUserId",
                table: "GeevUsers");

            migrationBuilder.DropIndex(
                name: "IX_GeevUsers_DeleterUserId",
                table: "GeevUsers");

            migrationBuilder.DropIndex(
                name: "IX_GeevUsers_LastModifierUserId",
                table: "GeevUsers");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "GeevUsers");

            migrationBuilder.DropColumn(
                name: "IsLockoutEnabled",
                table: "GeevUsers");

            migrationBuilder.DropColumn(
                name: "IsPhoneNumberConfirmed",
                table: "GeevUsers");

            migrationBuilder.DropColumn(
                name: "IsTwoFactorEnabled",
                table: "GeevUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEndDateUtc",
                table: "GeevUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "GeevUsers");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "GeevUsers");

            migrationBuilder.RenameColumn(
                name: "UserId2",
                table: "GeevTenants",
                newName: "LastModifierUserId1");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "GeevTenants",
                newName: "DeleterUserId1");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "GeevTenants",
                newName: "CreatorUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_GeevTenants_UserId2",
                table: "GeevTenants",
                newName: "IX_GeevTenants_LastModifierUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_GeevTenants_UserId1",
                table: "GeevTenants",
                newName: "IX_GeevTenants_DeleterUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_GeevTenants_UserId",
                table: "GeevTenants",
                newName: "IX_GeevTenants_CreatorUserId1");

            migrationBuilder.AlterColumn<string>(
                name: "EmailConfirmationCode",
                table: "GeevUsers",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 328,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeevUsers_CreatorUserId",
                table: "GeevUsers",
                column: "CreatorUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeevUsers_LastModifierUserId",
                table: "GeevUsers",
                column: "LastModifierUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GeevTenants_GeevUsers_CreatorUserId1",
                table: "GeevTenants",
                column: "CreatorUserId1",
                principalTable: "GeevUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeevTenants_GeevUsers_DeleterUserId1",
                table: "GeevTenants",
                column: "DeleterUserId1",
                principalTable: "GeevUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GeevTenants_GeevUsers_LastModifierUserId1",
                table: "GeevTenants",
                column: "LastModifierUserId1",
                principalTable: "GeevUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
