﻿using System.ComponentModel.DataAnnotations;
using Geev.Application.Editions;
using Geev.Authorization.Users;
using Geev.Domain.Entities;
using Geev.Domain.Entities.Auditing;

namespace Geev.MultiTenancy
{
    /// <summary>
    /// Represents a Tenant of the application.
    /// </summary>
    public abstract class GeevTenant<TUser> : GeevTenantBase, IFullAudited<TUser>
        where TUser : GeevUserBase
    {
        /// <summary>
        /// Current <see cref="Edition"/> of the Tenant.
        /// </summary>
        public virtual Edition Edition { get; set; }
        public virtual int? EditionId { get; set; }

        /// <summary>
        /// Reference to the creator user of this entity.
        /// </summary>
        public virtual TUser CreatorUser { get; set; }

        /// <summary>
        /// Reference to the last modifier user of this entity.
        /// </summary>
        public virtual TUser LastModifierUser { get; set; }

        /// <summary>
        /// Reference to the deleter user of this entity.
        /// </summary>
        public virtual TUser DeleterUser { get; set; }

        /// <summary>
        /// Creates a new tenant.
        /// </summary>
        protected GeevTenant()
        {
            IsActive = true;
        }

        /// <summary>
        /// Creates a new tenant.
        /// </summary>
        /// <param name="tenancyName">UNIQUE name of this Tenant</param>
        /// <param name="name">Display name of the Tenant</param>
        protected GeevTenant(string tenancyName, string name)
            : this()
        {
            TenancyName = tenancyName;
            Name = name;
        }
    }
}
