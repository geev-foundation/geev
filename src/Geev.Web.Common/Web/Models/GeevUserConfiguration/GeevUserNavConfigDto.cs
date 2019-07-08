using System.Collections.Generic;
using Geev.Application.Navigation;

namespace Geev.Web.Models.GeevUserConfiguration
{
    public class GeevUserNavConfigDto
    {
        public Dictionary<string, UserMenu> Menus { get; set; }
    }
}