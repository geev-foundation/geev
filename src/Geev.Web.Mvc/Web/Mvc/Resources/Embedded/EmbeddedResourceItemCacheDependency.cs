using System.Web.Caching;
using Geev.Resources.Embedded;

namespace Geev.Web.Mvc.Resources.Embedded
{
    public class EmbeddedResourceItemCacheDependency : CacheDependency
    {
        public EmbeddedResourceItemCacheDependency(EmbeddedResourceItem resource)
        {
            SetUtcLastModified(resource.LastModifiedUtc);
        }
    }
}