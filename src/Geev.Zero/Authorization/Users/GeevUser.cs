using System.ComponentModel.DataAnnotations;
using Geev.Domain.Entities.Auditing;
using Microsoft.AspNet.Identity;

namespace Geev.Authorization.Users
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public abstract class GeevUser<TUser> : GeevUserBase, IUser<long>, IFullAudited<TUser>
        where TUser : GeevUser<TUser>
    {
        /// <summary>
        /// User name.
        /// User name must be unique for it's tenant.
        /// </summary>
        [Required]
        [StringLength(MaxUserNameLength)]
        public virtual string NormalizedUserName { get; set; }

        /// <summary>
        /// Email address of the user.
        /// Email address must be unique for it's tenant.
        /// </summary>
        [Required]
        [StringLength(MaxEmailAddressLength)]
        public virtual string NormalizedEmailAddress { get; set; }

        public virtual TUser DeleterUser { get; set; }

        public virtual TUser CreatorUser { get; set; }

        public virtual TUser LastModifierUser { get; set; }

        protected GeevUser()
        {
        }

        public virtual void SetNormalizedNames()
        {
            NormalizedUserName = UserName.ToUpperInvariant();
            NormalizedEmailAddress = EmailAddress.ToUpperInvariant();
        }
    }
}