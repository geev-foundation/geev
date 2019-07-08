using System;
using Geev.Authorization.Users;

namespace Geev.Runtime.Session
{
    public static class GeevSessionExtensions
    {
        public static bool IsUser(this IGeevSession session, GeevUserBase user)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return session.TenantId == user.TenantId && 
                session.UserId.HasValue && 
                session.UserId.Value == user.Id;
        }
    }
}
