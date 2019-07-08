using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Geev.Authorization
{
    public interface IAuthorizationHelper
    {
        Task AuthorizeAsync(IEnumerable<IGeevAuthorizeAttribute> authorizeAttributes);

        Task AuthorizeAsync(MethodInfo methodInfo, Type type);
    }
}