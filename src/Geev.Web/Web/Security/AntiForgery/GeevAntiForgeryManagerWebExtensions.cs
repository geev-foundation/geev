using System.Reflection;
using Geev.Reflection;

namespace Geev.Web.Security.AntiForgery
{
    public static class GeevAntiForgeryManagerWebExtensions
    {
        public static bool ShouldValidate(
            this IGeevAntiForgeryManager manager,
            IGeevAntiForgeryWebConfiguration antiForgeryWebConfiguration,
            MethodInfo methodInfo, 
            HttpVerb httpVerb, 
            bool defaultValue)
        {
            if (!antiForgeryWebConfiguration.IsEnabled)
            {
                return false;
            }

            if (methodInfo.IsDefined(typeof(ValidateGeevAntiForgeryTokenAttribute), true))
            {
                return true;
            }

            if (ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableGeevAntiForgeryTokenValidationAttribute>(methodInfo) != null)
            {
                return false;
            }

            if (antiForgeryWebConfiguration.IgnoredHttpVerbs.Contains(httpVerb))
            {
                return false;
            }

            if (methodInfo.DeclaringType?.IsDefined(typeof(ValidateGeevAntiForgeryTokenAttribute), true) ?? false)
            {
                return true;
            }

            return defaultValue;
        }
    }
}
