using System.Collections.Generic;

namespace Geev.Web.Security.AntiForgery
{
    public class GeevAntiForgeryWebConfiguration : IGeevAntiForgeryWebConfiguration
    {
        public bool IsEnabled { get; set; }

        public HashSet<HttpVerb> IgnoredHttpVerbs { get; }

        public GeevAntiForgeryWebConfiguration()
        {
            IsEnabled = true;
            IgnoredHttpVerbs = new HashSet<HttpVerb> { HttpVerb.Get, HttpVerb.Head, HttpVerb.Options, HttpVerb.Trace };
        }
    }
}