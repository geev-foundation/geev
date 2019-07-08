using System;
using Geev.Authorization.Roles;
using Geev.Threading;

namespace Geev.Authorization.Users
{
    /// <summary>
    /// Extension methods for <see cref="GeevUserManager{TRole,TUser}"/>.
    /// </summary>
    public static class GeevUserManagerExtensions
    {
        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="manager">User manager</param>
        /// <param name="userId">User id</param>
        /// <param name="permissionName">Permission name</param>
        public static bool IsGranted<TRole, TUser>(GeevUserManager<TRole, TUser> manager, long userId, string permissionName)
            where TRole : GeevRole<TUser>, new()
            where TUser : GeevUser<TUser>
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            return AsyncHelper.RunSync(() => manager.IsGrantedAsync(userId, permissionName));
        }
    }
}