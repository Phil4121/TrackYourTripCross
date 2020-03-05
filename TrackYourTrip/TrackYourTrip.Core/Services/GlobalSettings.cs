using System;
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
    }
}
