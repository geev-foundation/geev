﻿using System.Data.Entity;
using Geev.Application.Editions;
using Geev.Application.Features;
using Geev.Auditing;
using Geev.Authorization;
using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.EntityHistory;
using Geev.BackgroundJobs;
using Geev.Configuration;
using Geev.Localization;
using Geev.MultiTenancy;
using Geev.Notifications;
using Geev.Organizations;

namespace Geev.Zero.EntityFramework
{
    /// <summary>
    /// Extension methods for <see cref="DbModelBuilder"/>.
    /// </summary>
    public static class GeevZeroDbModelBuilderExtensions
    {
        /// <summary>
        /// Changes prefix for ABP tables (which is "Geev" by default).
        /// Can be null/empty string to clear the prefix.
        /// </summary>
        /// <typeparam name="TTenant">The type of the tenant entity.</typeparam>
        /// <typeparam name="TRole">The type of the role entity.</typeparam>
        /// <typeparam name="TUser">The type of the user entity.</typeparam>
        /// <param name="modelBuilder">Model builder.</param>
        /// <param name="prefix">Table prefix, or null to clear prefix.</param>
        public static void ChangeGeevTablePrefix<TTenant, TRole, TUser>(this DbModelBuilder modelBuilder, string prefix, string schemaName = null)
            where TTenant : GeevTenant<TUser>
            where TRole : GeevRole<TUser>
            where TUser : GeevUser<TUser>
        {
            prefix = prefix ?? "";

            SetTableName<AuditLog>(modelBuilder, prefix + "AuditLogs", schemaName);
            SetTableName<BackgroundJobInfo>(modelBuilder, prefix + "BackgroundJobs", schemaName);
            SetTableName<Edition>(modelBuilder, prefix + "Editions", schemaName);
            SetTableName<EntityChange>(modelBuilder, prefix + "EntityChanges", schemaName);
            SetTableName<EntityChangeSet>(modelBuilder, prefix + "EntityChangeSets", schemaName);
            SetTableName<EntityPropertyChange>(modelBuilder, prefix + "EntityPropertyChanges", schemaName);
            SetTableName<FeatureSetting>(modelBuilder, prefix + "Features", schemaName);
            SetTableName<TenantFeatureSetting>(modelBuilder, prefix + "Features", schemaName);
            SetTableName<EditionFeatureSetting>(modelBuilder, prefix + "Features", schemaName);
            SetTableName<ApplicationLanguage>(modelBuilder, prefix + "Languages", schemaName);
            SetTableName<ApplicationLanguageText>(modelBuilder, prefix + "LanguageTexts", schemaName);
            SetTableName<NotificationInfo>(modelBuilder, prefix + "Notifications", schemaName);
            SetTableName<NotificationSubscriptionInfo>(modelBuilder, prefix + "NotificationSubscriptions", schemaName);
            SetTableName<OrganizationUnit>(modelBuilder, prefix + "OrganizationUnits", schemaName);
            SetTableName<PermissionSetting>(modelBuilder, prefix + "Permissions", schemaName);
            SetTableName<RolePermissionSetting>(modelBuilder, prefix + "Permissions", schemaName);
            SetTableName<UserPermissionSetting>(modelBuilder, prefix + "Permissions", schemaName);
            SetTableName<TRole>(modelBuilder, prefix + "Roles", schemaName);
            SetTableName<Setting>(modelBuilder, prefix + "Settings", schemaName);
            SetTableName<TTenant>(modelBuilder, prefix + "Tenants", schemaName);
            SetTableName<UserLogin>(modelBuilder, prefix + "UserLogins", schemaName);
            SetTableName<UserLoginAttempt>(modelBuilder, prefix + "UserLoginAttempts", schemaName);
            SetTableName<TenantNotificationInfo>(modelBuilder, prefix + "TenantNotifications", schemaName);
            SetTableName<UserNotificationInfo>(modelBuilder, prefix + "UserNotifications", schemaName);
            SetTableName<UserOrganizationUnit>(modelBuilder, prefix + "UserOrganizationUnits", schemaName);
            SetTableName<OrganizationUnitRole>(modelBuilder, prefix + "OrganizationUnitRoles", schemaName);
            SetTableName<UserRole>(modelBuilder, prefix + "UserRoles", schemaName);
            SetTableName<TUser>(modelBuilder, prefix + "Users", schemaName);
            SetTableName<UserAccount>(modelBuilder, prefix + "UserAccounts", schemaName);
            SetTableName<UserClaim>(modelBuilder, prefix + "UserClaims", schemaName);
        }

        private static void SetTableName<TEntity>(DbModelBuilder modelBuilder, string tableName, string schemaName)
            where TEntity : class
        {
            if (schemaName == null)
            {
                modelBuilder.Entity<TEntity>().ToTable(tableName);
            }
            else
            {
                modelBuilder.Entity<TEntity>().ToTable(tableName, schemaName);                
            }
        }
    }
}