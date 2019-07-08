namespace Geev.Web.Security.AntiForgery
{
    public interface IGeevAntiForgeryManager
    {
        IGeevAntiForgeryConfiguration Configuration { get; }

        string GenerateToken();
    }
}
