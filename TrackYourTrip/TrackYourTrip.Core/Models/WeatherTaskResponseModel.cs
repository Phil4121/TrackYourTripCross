using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    public class WeatherTaskResponseModel
    {
        public WeatherTaskResponseModel()
        {
            Success = false;
        }

        public bool Success {get;set;}

        public double Temperature { get; set; }

        public int TemperatureUnit { get; set; }
    }
}
