// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Resources;

namespace TrackYourTrip.Core.Helpers
{
    public static class TranslateExtension
    {
        public static string Translate(this ILocalizeService localizeService, string str)
        {
            string translation = AppResources.ResourceManager.GetString(str, localizeService.GetCurrentCultureInfo());
            return string.IsNullOrEmpty(translation) ? str : translation;
        }
    }
}
