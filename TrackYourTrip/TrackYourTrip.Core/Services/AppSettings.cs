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
        private string _darkSkySerial = "XXXX";
        private string _darkSkySerialDefault = string.Empty;

        public string DarkSkySerial
        {
            get => Preferences.Get(_darkSkySerial, _darkSkySerialDefault);
            set => Preferences.Set(_darkSkySerial, value);
        }

    }
}
