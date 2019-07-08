using System.Collections.Generic;
using Geev.Localization;

namespace Geev.Web.Models.GeevUserConfiguration
{
    public class GeevUserLocalizationConfigDto
    {
        public GeevUserCurrentCultureConfigDto CurrentCulture { get; set; }

        public List<LanguageInfo> Languages { get; set; }

        public LanguageInfo CurrentLanguage { get; set; }

        public List<GeevLocalizationSourceDto> Sources { get; set; }

        public Dictionary<string, Dictionary<string, string>> Values { get; set; }
    }
}