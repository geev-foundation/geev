namespace Geev.Web.Security.AntiForgery
{
    /// <summary>
    /// This interface is internally used by ABP framework and normally should not be used by applications.
    /// If it's needed, use 
    /// <see cref="IGeevAntiForgeryManager"/> and cast to 
    /// <see cref="IGeevAntiForgeryValidator"/> to use 
    /// <see cref="IsValid"/> method.
    /// </summary>
    public interface IGeevAntiForgeryValidator
    {
        bool IsValid(string cookieValue, string tokenValue);
    }
}