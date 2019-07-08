using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Geev.Zero.SampleApp.EntityFrameworkCore;

namespace Geev.Zero.SampleApp.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20160901080154_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Geev.Application.Editions.Edition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long?>("DeleterUserId");

                    b.Property<DateTime?>("DeletionTime");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 32);

                    b.HasKey("Id");

                    b.ToTable("GeevEditions");
                });

            modelBuilder.Entity("Geev.Application.Features.FeatureSetting", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 2000);

                    b.HasKey("Id");

                    b.ToTable("GeevFeatures");

                    b.HasDiscriminator<string>("Discriminator").HasValue("FeatureSetting");
                });

            modelBuilder.Entity("Geev.Auditing.AuditLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BrowserInfo")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("ClientIpAddress")
                        .HasAnnotation("MaxLength", 64);

                    b.Property<string>("ClientName")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("CustomData")
                        .HasAnnotation("MaxLength", 2000);

                    b.Property<string>("Exception")
                        .HasAnnotation("MaxLength", 2000);

                    b.Property<int>("ExecutionDuration");

                    b.Property<DateTime>("ExecutionTime");

                    b.Property<int?>("ImpersonatorTenantId");

                    b.Property<long?>("ImpersonatorUserId");

                    b.Property<string>("MethodName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("Parameters")
                        .HasAnnotation("MaxLength", 1024);

                    b.Property<string>("ServiceName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int?>("TenantId");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.ToTable("GeevAuditLogs");
                });

            modelBuilder.Entity("Geev.Authorization.PermissionSetting", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<bool>("IsGranted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.ToTable("GeevPermissions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("PermissionSetting");
                });

            modelBuilder.Entity("Geev.Authorization.Users.UserAccount", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long?>("DeleterUserId");

                    b.Property<DateTime?>("DeletionTime");

                    b.Property<string>("EmailAddress");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastLoginTime");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<int?>("TenantId");

                    b.Property<long>("UserId");

                    b.Property<long?>("UserLinkId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("GeevUserAccounts");
                });

            modelBuilder.Entity("Geev.Authorization.Users.UserLogin", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("ProviderKey")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int?>("TenantId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("GeevUserLogins");
                });

            modelBuilder.Entity("Geev.Authorization.Users.UserLoginAttempt", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BrowserInfo")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("ClientIpAddress")
                        .HasAnnotation("MaxLength", 64);

                    b.Property<string>("ClientName")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<DateTime>("CreationTime");

                    b.Property<byte>("Result");

                    b.Property<string>("TenancyName")
                        .HasAnnotation("MaxLength", 64);

                    b.Property<int?>("TenantId");

                    b.Property<long?>("UserId");

                    b.Property<string>("UserNameOrEmailAddress")
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.HasIndex("UserId", "TenantId");

                    b.HasIndex("TenancyName", "UserNameOrEmailAddress", "Result");

                    b.ToTable("GeevUserLoginAttempts");
                });

            modelBuilder.Entity("Geev.Authorization.Users.UserOrganizationUnit", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long>("OrganizationUnitId");

                    b.Property<int?>("TenantId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.ToTable("GeevUserOrganizationUnits");
                });

            modelBuilder.Entity("Geev.Authorization.Users.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<int>("RoleId");

                    b.Property<int?>("TenantId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("GeevUserRoles");
                });

            modelBuilder.Entity("Geev.BackgroundJobs.BackgroundJobInfo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<bool>("IsAbandoned");

                    b.Property<string>("JobArgs")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 1048576);

                    b.Property<string>("JobType")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 512);

                    b.Property<DateTime?>("LastTryTime");

                    b.Property<DateTime>("NextTryTime");

                    b.Property<byte>("Priority");

                    b.Property<short>("TryCount");

                    b.HasKey("Id");

                    b.HasIndex("IsAbandoned", "NextTryTime");

                    b.ToTable("GeevBackgroundJobs");
                });

            modelBuilder.Entity("Geev.Configuration.Setting", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int?>("TenantId");

                    b.Property<long?>("UserId");

                    b.Property<string>("Value")
                        .HasAnnotation("MaxLength", 2000);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("GeevSettings");
                });

            modelBuilder.Entity("Geev.Localization.ApplicationLanguage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long?>("DeleterUserId");

                    b.Property<DateTime?>("DeletionTime");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.Property<string>("Icon")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.ToTable("GeevLanguages");
                });

            modelBuilder.Entity("Geev.Localization.ApplicationLanguageText", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("LanguageName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<int?>("TenantId");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 67108864);

                    b.HasKey("Id");

                    b.ToTable("GeevLanguageTexts");
                });

            modelBuilder.Entity("Geev.Notifications.NotificationInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<string>("Data")
                        .HasAnnotation("MaxLength", 1048576);

                    b.Property<string>("DataTypeName")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<string>("EntityId")
                        .HasAnnotation("MaxLength", 96);

                    b.Property<string>("EntityTypeAssemblyQualifiedName")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<string>("EntityTypeName")
                        .HasAnnotation("MaxLength", 250);

                    b.Property<string>("ExcludedUserIds")
                        .HasAnnotation("MaxLength", 131072);

                    b.Property<string>("NotificationName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 96);

                    b.Property<byte>("Severity");

                    b.Property<string>("TenantIds")
                        .HasAnnotation("MaxLength", 131072);

                    b.Property<string>("UserIds")
                        .HasAnnotation("MaxLength", 131072);

                    b.HasKey("Id");

                    b.ToTable("GeevNotifications");
                });

            modelBuilder.Entity("Geev.Notifications.NotificationSubscriptionInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<string>("EntityId")
                        .HasAnnotation("MaxLength", 96);

                    b.Property<string>("EntityTypeAssemblyQualifiedName")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<string>("EntityTypeName")
                        .HasAnnotation("MaxLength", 250);

                    b.Property<string>("NotificationName")
                        .HasAnnotation("MaxLength", 96);

                    b.Property<int?>("TenantId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("NotificationName", "EntityTypeName", "EntityId", "UserId");

                    b.ToTable("GeevNotificationSubscriptions");
                });

            modelBuilder.Entity("Geev.Notifications.TenantNotificationInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<string>("Data")
                        .HasAnnotation("MaxLength", 1048576);

                    b.Property<string>("DataTypeName")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<string>("EntityId")
                        .HasAnnotation("MaxLength", 96);

                    b.Property<string>("EntityTypeAssemblyQualifiedName")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<string>("EntityTypeName")
                        .HasAnnotation("MaxLength", 250);

                    b.Property<string>("NotificationName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 96);

                    b.Property<byte>("Severity");

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.ToTable("GeevTenantNotifications");
                });

            modelBuilder.Entity("Geev.Notifications.UserNotificationInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<int>("State");

                    b.Property<int?>("TenantId");

                    b.Property<Guid>("TenantNotificationId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "State", "CreationTime");

                    b.ToTable("GeevUserNotifications");
                });

            modelBuilder.Entity("Geev.Organizations.OrganizationUnit", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 95);

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long?>("DeleterUserId");

                    b.Property<DateTime?>("DeletionTime");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<long?>("ParentId");

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("GeevOrganizationUnits");
                });

            modelBuilder.Entity("Geev.Zero.SampleApp.MultiTenancy.Tenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConnectionString")
                        .HasAnnotation("MaxLength", 1024);

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long?>("CreatorUserId1");

                    b.Property<long?>("DeleterUserId");

                    b.Property<long?>("DeleterUserId1");

                    b.Property<DateTime?>("DeletionTime");

                    b.Property<int?>("EditionId");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<long?>("LastModifierUserId1");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("TenancyName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.HasKey("Id");

                    b.HasIndex("CreatorUserId1");

                    b.HasIndex("DeleterUserId1");

                    b.HasIndex("EditionId");

                    b.HasIndex("LastModifierUserId1");

                    b.ToTable("GeevTenants");
                });

            modelBuilder.Entity("Geev.Zero.SampleApp.Roles.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long?>("DeleterUserId");

                    b.Property<DateTime?>("DeletionTime");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.Property<bool>("IsDefault");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsStatic");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 32);

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("CreatorUserId");

                    b.HasIndex("DeleterUserId");

                    b.HasIndex("LastModifierUserId");

                    b.ToTable("GeevRoles");
                });

            modelBuilder.Entity("Geev.Zero.SampleApp.Users.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthenticationSource")
                        .HasAnnotation("MaxLength", 64);

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long?>("DeleterUserId");

                    b.Property<DateTime?>("DeletionTime");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("EmailConfirmationCode")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEmailConfirmed");

                    b.Property<DateTime?>("LastLoginTime");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 32);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("PasswordResetCode")
                        .HasAnnotation("MaxLength", 328);

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 32);

                    b.Property<int?>("TenantId");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 32);

                    b.HasKey("Id");

                    b.HasIndex("CreatorUserId")
                        .IsUnique();

                    b.HasIndex("LastModifierUserId")
                        .IsUnique();

                    b.ToTable("GeevUsers");
                });

            modelBuilder.Entity("Geev.Application.Features.EditionFeatureSetting", b =>
                {
                    b.HasBaseType("Geev.Application.Features.FeatureSetting");

                    b.Property<int>("EditionId");

                    b.HasIndex("EditionId");

                    b.ToTable("GeevFeatures");

                    b.HasDiscriminator().HasValue("EditionFeatureSetting");
                });

            modelBuilder.Entity("Geev.MultiTenancy.TenantFeatureSetting", b =>
                {
                    b.HasBaseType("Geev.Application.Features.FeatureSetting");

                    b.Property<int>("TenantId");

                    b.ToTable("GeevFeatures");

                    b.HasDiscriminator().HasValue("TenantFeatureSetting");
                });

            modelBuilder.Entity("Geev.Authorization.Roles.RolePermissionSetting", b =>
                {
                    b.HasBaseType("Geev.Authorization.PermissionSetting");

                    b.Property<int>("RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("GeevPermissions");

                    b.HasDiscriminator().HasValue("RolePermissionSetting");
                });

            modelBuilder.Entity("Geev.Authorization.Users.UserPermissionSetting", b =>
                {
                    b.HasBaseType("Geev.Authorization.PermissionSetting");

                    b.Property<long>("UserId");

                    b.HasIndex("UserId");

                    b.ToTable("GeevPermissions");

                    b.HasDiscriminator().HasValue("UserPermissionSetting");
                });

            modelBuilder.Entity("Geev.Authorization.Users.UserLogin", b =>
                {
                    b.HasOne("Geev.Zero.SampleApp.Users.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Geev.Authorization.Users.UserRole", b =>
                {
                    b.HasOne("Geev.Zero.SampleApp.Users.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Geev.Configuration.Setting", b =>
                {
                    b.HasOne("Geev.Zero.SampleApp.Users.User")
                        .WithMany("Settings")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Geev.Organizations.OrganizationUnit", b =>
                {
                    b.HasOne("Geev.Organizations.OrganizationUnit", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("Geev.Zero.SampleApp.MultiTenancy.Tenant", b =>
                {
                    b.HasOne("Geev.Zero.SampleApp.Users.User", "CreatorUser")
                        .WithMany()
                        .HasForeignKey("CreatorUserId1");

                    b.HasOne("Geev.Zero.SampleApp.Users.User", "DeleterUser")
                        .WithMany()
                        .HasForeignKey("DeleterUserId1");

                    b.HasOne("Geev.Application.Editions.Edition", "Edition")
                        .WithMany()
                        .HasForeignKey("EditionId");

                    b.HasOne("Geev.Zero.SampleApp.Users.User", "LastModifierUser")
                        .WithMany()
                        .HasForeignKey("LastModifierUserId1");
                });

            modelBuilder.Entity("Geev.Zero.SampleApp.Roles.Role", b =>
                {
                    b.HasOne("Geev.Zero.SampleApp.Users.User", "CreatorUser")
                        .WithMany()
                        .HasForeignKey("CreatorUserId");

                    b.HasOne("Geev.Zero.SampleApp.Users.User", "DeleterUser")
                        .WithMany()
                        .HasForeignKey("DeleterUserId");

                    b.HasOne("Geev.Zero.SampleApp.Users.User", "LastModifierUser")
                        .WithMany()
                        .HasForeignKey("LastModifierUserId");
                });

            modelBuilder.Entity("Geev.Zero.SampleApp.Users.User", b =>
                {
                    b.HasOne("Geev.Zero.SampleApp.Users.User", "CreatorUser")
                        .WithOne()
                        .HasForeignKey("Geev.Zero.SampleApp.Users.User", "CreatorUserId");

                    b.HasOne("Geev.Zero.SampleApp.Users.User", "LastModifierUser")
                        .WithOne()
                        .HasForeignKey("Geev.Zero.SampleApp.Users.User", "LastModifierUserId");
                });

            modelBuilder.Entity("Geev.Application.Features.EditionFeatureSetting", b =>
                {
                    b.HasOne("Geev.Application.Editions.Edition", "Edition")
                        .WithMany()
                        .HasForeignKey("EditionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Geev.Authorization.Roles.RolePermissionSetting", b =>
                {
                    b.HasOne("Geev.Zero.SampleApp.Roles.Role")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Geev.Authorization.Users.UserPermissionSetting", b =>
                {
                    b.HasOne("Geev.Zero.SampleApp.Users.User")
                        .WithMany("Permissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
