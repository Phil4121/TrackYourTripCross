using System;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using Xamarin.Essentials;

namespace TrackYourTrip.Core.Services
{
    public class GlobalSettings : IAppSettings
    {
        public string DarkSkySerial { get; } = "8cdd0e39ef33b342eaa8168553ff4107";

        public int DefaultThreadWaitTime { get; } = 5000;

        public string TripIdInProcess
        {
            get => Preferences.Get(nameof(TripIdInProcess), Guid.Empty.ToString());
            set => Preferences.Set(nameof(TripIdInProcess), value);
        }

        public PreDefinedSpotSettings PreDefinedSpotSettings
        {
            get
            {
                try
                {
                    var settingString = Preferences.Get(nameof(PreDefinedSpotSettings), string.Empty);

                    if (!string.IsNullOrEmpty(settingString))
                    {
                        return new JSONHelper<PreDefinedSpotSettings>().Deserialize(settingString);
                    }

                    return new PreDefinedSpotSettings();

                }catch(Exception ex)
                {
                    throw ex;
                }
            }

            set
            {
                Preferences.Set(nameof(PreDefinedSpotSettings), new JSONHelper<PreDefinedSpotSettings>().Serialize(value));
            }
        }
    }
}
