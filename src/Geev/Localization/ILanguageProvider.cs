using System.Collections.Generic;

namespace Geev.Localization
{
    public interface ILanguageProvider
    {
        IReadOnlyList<LanguageInfo> GetLanguages();
    }
}