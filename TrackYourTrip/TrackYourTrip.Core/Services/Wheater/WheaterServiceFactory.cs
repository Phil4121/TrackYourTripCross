using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Services.Wheater.DarkSky;

namespace TrackYourTrip.Core.Services.Wheater
{
    public static class WheaterServiceFactory
    {
        public static IWheaterService GetWheaterServiceFactory()
        {
            return new DarkSkyWheaterService();
        }
    }
}
