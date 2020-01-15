// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using Xamarin.Essentials;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.Services
{
    public class AppSettings : IAppSettings
    {
        public const string SuperNumberKey = "SuperNumberKey";
        public const int SuperNumberDefaultValue = 1;

        public int SuperNumber
        {
            get { return Preferences.Get(SuperNumberKey, SuperNumberDefaultValue); }
            set { Preferences.Set(SuperNumberKey, value); }
        }
    }
}
