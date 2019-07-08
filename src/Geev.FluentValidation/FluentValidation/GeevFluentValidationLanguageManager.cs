using System.Collections.Generic;
using System.Globalization;
using Geev.Extensions;
using Geev.FluentValidation.Configuration;
using Geev.Localization;
using Geev.Localization.Sources;
using LanguageManager = FluentValidation.Resources.LanguageManager;

namespace Geev.FluentValidation
{
    public class GeevFluentValidationLanguageManager : LanguageManager
    {
        public GeevFluentValidationLanguageManager(
            ILocalizationManager localizationManager,
            ILanguageManager languageManager,
            IGeevFluentValidationConfiguration configuration)
        {
            if (!configuration.LocalizationSourceName.IsNullOrEmpty())
            {
                var source = localizationManager.GetSource(configuration.LocalizationSourceName);
                var languages = languageManager.GetLanguages();

                AddAllTranslations(source, languages);
            }
        }

        private void AddAllTranslations(ILocalizationSource source, IReadOnlyList<LanguageInfo> languages)
        {
            foreach (var language in languages)
            {
                var culture = new CultureInfo(language.Name);
                var translations = source.GetAllStrings(culture, false);
                AddTranslations(language.Name, translations);
            }
        }

        private void AddTranslations(string language, IReadOnlyList<LocalizedString> translations)
        {
            foreach (var translation in translations)
            {
                AddTranslation(language, translation.Name, translation.Value);
            }
        }
    }
}
