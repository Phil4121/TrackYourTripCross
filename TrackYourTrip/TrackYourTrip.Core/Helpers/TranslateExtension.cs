// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using TrackYourTrip.Core.Resources;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.Helpers
{
    public static class TranslateExtension
    {
        public static string Translate(this ILocalizeService localizeService, string str)
        {
            var translation = AppResources.ResourceManager.GetString(str, localizeService.GetCurrentCultureInfo());
            return string.IsNullOrEmpty(translation) ? str : translation;
        }
    }
}
