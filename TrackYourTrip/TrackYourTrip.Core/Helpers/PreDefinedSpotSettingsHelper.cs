using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Services;

namespace TrackYourTrip.Core.Helpers
{
    public class PreDefinedSpotSettings
    {
        public double WaterTemperature { get; set; }
        public int WaterTemperatureUnit { get; set; }

        public double WaterLevel { get; set; }
        public int WaterLevelUnit { get; set; }
    }
}
