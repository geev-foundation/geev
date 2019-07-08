using System.Collections.Generic;

namespace Geev.Web.Models.GeevUserConfiguration
{
    public class GeevUserAuthConfigDto
    {
        public Dictionary<string,string> AllPermissions { get; set; }

        public Dictionary<string, string> GrantedPermissions { get; set; }
        
    }
}