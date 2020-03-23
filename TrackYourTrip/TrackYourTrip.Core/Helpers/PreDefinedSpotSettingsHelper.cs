using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services;

namespace TrackYourTrip.Core.Helpers
{
    public class PreDefinedSpotSettings
    {
        public double WaterTemperature { get; set; }
        public int WaterTemperatureUnit { get; set; }

        public double WaterLevel { get; set; }
        public int WaterLevelUnit { get; set; }

        public Guid ID_Turbidity { get; set; }
        public Guid ID_WaterColor { get; set; }
        public Guid ID_Current { get; set; }

        public PreDefinedSpotSettings()
        {
            ID_Turbidity = Guid.NewGuid();
            ID_WaterColor = Guid.NewGuid();
            ID_Current = Guid.NewGuid();
        }
    }
}
