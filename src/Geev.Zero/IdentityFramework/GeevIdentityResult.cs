using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace Geev.IdentityFramework
{
    public class GeevIdentityResult : IdentityResult
    {
        public GeevIdentityResult()
        {
            
        }

        public GeevIdentityResult(IEnumerable<string> errors)
            : base(errors)
        {
            
        }

        public GeevIdentityResult(params string[] errors)
            :base(errors)
        {
            
        }

        public static GeevIdentityResult Failed(params string[] errors)
        {
            return new GeevIdentityResult(errors);
        }
    }
}