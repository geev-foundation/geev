using System;
using Geev.Dependency;
using Castle.Core.Logging;

namespace Geev.Web.Security.AntiForgery
{
    public class GeevAntiForgeryManager : IGeevAntiForgeryManager, IGeevAntiForgeryValidator, ITransientDependency
    {
        public ILogger Logger { protected get; set; }

        public IGeevAntiForgeryConfiguration Configuration { get; }

        public GeevAntiForgeryManager(IGeevAntiForgeryConfiguration configuration)
        {
            Configuration = configuration;
            Logger = NullLogger.Instance;
        }

        public virtual string GenerateToken()
        {
            return Guid.NewGuid().ToString("D");
        }

        public virtual bool IsValid(string cookieValue, string tokenValue)
        {
            return cookieValue == tokenValue;
        }
    }
}