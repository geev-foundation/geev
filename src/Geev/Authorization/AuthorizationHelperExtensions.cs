using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Geev.Threading;

namespace Geev.Authorization
{
    public static class AuthorizationHelperExtensions
    {
        public static async Task AuthorizeAsync(this IAuthorizationHelper authorizationHelper, IGeevAuthorizeAttribute authorizeAttribute)
        {
            await authorizationHelper.AuthorizeAsync(new[] { authorizeAttribute });
        }

        public static void Authorize(this IAuthorizationHelper authorizationHelper, IEnumerable<IGeevAuthorizeAttribute> authorizeAttributes)
        {
            AsyncHelper.RunSync(() => authorizationHelper.AuthorizeAsync(authorizeAttributes));
        }

        public static void Authorize(this IAuthorizationHelper authorizationHelper, IGeevAuthorizeAttribute authorizeAttribute)
        {
            authorizationHelper.Authorize(new[] { authorizeAttribute });
        }

        public static void Authorize(this IAuthorizationHelper authorizationHelper, MethodInfo methodInfo, Type type)
        {
            AsyncHelper.RunSync(() => authorizationHelper.AuthorizeAsync(methodInfo, type));
        }
    }
}