using System;

namespace Geev.Web.Security.AntiForgery
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
    public class DisableGeevAntiForgeryTokenValidationAttribute : Attribute
    {

    }
}
