using MvvmCross;
using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Interfaces;
using Xamarin.Essentials;

namespace TrackYourTrip.Core.Helpers
{
    public static class TripHelper
    {
        private static IAppSettings settings = Mvx.IoCProvider.Resolve<IAppSettings>();

        public static string GetTripIdInProcess()
        {
            return settings.TripIdInProcess;
        }

        public static bool TripInProcess()
        {
            return Guid.Parse(settings.TripIdInProcess) != Guid.Empty;
        }

        public static void ResetTripInProcess()
        {
            settings.TripIdInProcess = Guid.Empty.ToString();
        }

        public static void ResetPreSettings()
        {
            settings.PreDefinedSpotSettings = new PreDefinedSpotSettings();
            Preferences.Clear();
        }
    }
}
