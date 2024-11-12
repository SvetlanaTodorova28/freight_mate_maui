using Mde.Project.Mobile.Domain.Models;

namespace Mde.Project.Mobile.Helpers;

public static class LanguageHelper
{
    public static readonly Dictionary<LanguageOption, string> LanguageCodes = new()
    {
        { LanguageOption.English, "en-US" },
        { LanguageOption.Dutch, "nl-NL" },
        { LanguageOption.French, "fr-FR" },
        { LanguageOption.German, "de-DE" }
    };
}