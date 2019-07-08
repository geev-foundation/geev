using System;

namespace Geev.Authorization
{
    /// <summary>
    /// Used to allow a method to be accessed by any user.
    /// Suppress <see cref="GeevAuthorizeAttribute"/> defined in the class containing that method.
    /// </summary>
    public class GeevAllowAnonymousAttribute : Attribute, IGeevAllowAnonymousAttribute
    {

    }
}