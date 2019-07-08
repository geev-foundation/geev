﻿using Geev.Authorization.Users;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.NHibernate.EntityMappings
{
    public abstract class GeevUserMap<TUser> : EntityMap<TUser, long>
        where TUser : GeevUser<TUser>
    {
        protected GeevUserMap()
            : base("GeevUsers")
        {
            Map(x => x.TenantId);
            Map(x => x.UserName);
            Map(x => x.NormalizedUserName);
            Map(x => x.Name);
            Map(x => x.Surname);
            Map(x => x.EmailAddress);
            Map(x => x.NormalizedEmailAddress);
            Map(x => x.IsEmailConfirmed);
            Map(x => x.EmailConfirmationCode);
            Map(x => x.Password);
            Map(x => x.PasswordResetCode);
            Map(x => x.IsActive);
            Map(x => x.AuthenticationSource);
            Map(x => x.IsLockoutEnabled);
            Map(x => x.LockoutEndDateUtc);
            Map(x => x.AccessFailedCount);
            Map(x => x.PhoneNumber);
            Map(x => x.IsPhoneNumberConfirmed);
            Map(x => x.SecurityStamp);
            Map(x => x.IsTwoFactorEnabled);

            this.MapFullAudited();

            Polymorphism.Explicit();
        }
    }
}
