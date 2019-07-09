using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Geev.Authorization.Users;
using Geev.Domain.Entities;
using Geev.Domain.Entities.Auditing;

namespace Geev.Authorization.Roles
{
    /// <summary>
    /// Base class for role.
    /// </summary>
    [Table("Roles")]
    public abstract class GeevRoleBase : FullAuditedEntity<int>, IMayHaveTenant
    {
        /// <summary>
        /// Maximum length of the <see cref="DisplayName"/> property.
        /// </summary>
        public const int MaxDisplayNameLength = 64;

        /// <summary>
        /// Maximum length of the <see cref="Name"/> property.
        /// </summary>
        public const int MaxNameLength = 32;

        /// <summary>
        /// Tenant's Id, if this role is a tenant-level role. Null, if not.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Unique name of this role.
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Display name of this role.
        /// </summary>
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Is this a static role?
        /// Static roles can not be deleted, can not change their name.
        /// They can be used programmatically.
        /// </summary>
        public virtual bool IsStatic { get; set; }

        /// <summary>
        /// Is this role will be assigned to new users as default?
        /// </summary>
        public virtual bool IsDefault { get; set; }

        /// <summary>
        /// List of permissions of the role.
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual ICollection<RolePermissionSetting> Permissions { get; set; }

        protected GeevRoleBase()
        {
            Name = Guid.NewGuid().ToString("N");
        }

        protected GeevRoleBase(int? tenantId, string displayName)
            : this()
        {
            TenantId = tenantId;
            DisplayName = displayName;
        }

        protected GeevRoleBase(int? tenantId, string name, string displayName)
            : this(tenantId, displayName)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"[Role {Id}, Name={Name}]";
        }
    }
}