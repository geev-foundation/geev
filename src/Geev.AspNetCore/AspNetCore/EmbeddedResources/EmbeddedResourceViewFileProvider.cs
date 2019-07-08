using Geev.Dependency;
using Geev.Resources.Embedded;

namespace Geev.AspNetCore.EmbeddedResources
{
    public class EmbeddedResourceViewFileProvider : EmbeddedResourceFileProvider
    {
        public EmbeddedResourceViewFileProvider(IIocResolver iocResolver) 
            : base(iocResolver)
        {
        }

        protected override bool IsIgnoredFile(EmbeddedResourceItem resource)
        {
            return resource.FileExtension != "cshtml";
        }
    }
}