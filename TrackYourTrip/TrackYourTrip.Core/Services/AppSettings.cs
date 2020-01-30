// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using TrackYourTrip.Core.Interfaces;
using Xamarin.Essentials;

namespace TrackYourTrip.Core.Services
{
    public class AppSettings : IAppSettings
    {
        public const string SuperNumberKey = "SuperNumberKey";
        public const int SuperNumberDefaultValue = 1;

        public int SuperNumber
        {
            get => Preferences.Get(SuperNumberKey, SuperNumberDefaultValue);
            set => Preferences.Set(SuperNumberKey, value);
        }
    }
}
