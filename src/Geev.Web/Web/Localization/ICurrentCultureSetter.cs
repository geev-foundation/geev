using System.Web;

namespace Geev.Web.Localization
{
    public interface ICurrentCultureSetter
    {
        void SetCurrentCulture(HttpContext httpContext);
    }
}
