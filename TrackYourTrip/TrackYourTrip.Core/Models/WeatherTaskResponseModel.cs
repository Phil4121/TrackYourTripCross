using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Helpers;
using static TrackYourTrip.Core.Services.WeatherConditions;

namespace TrackYourTrip.Core.Models
{
    public class WeatherTaskResponseModel
    {
        public WeatherTaskResponseModel()
        {
            Success = false;
        }

        public bool Success {get;set;}

        public int WeatherSituation { get; set; }

        public double CurrentTemperature { get; set; }

        public double DailyTemperatureHigh { get; set; }

        public DateTime DailyTemperatureHighTime { get; set; }

        public double DailyTemperatureLow { get; set; }

        public DateTime DailyTemperatureLowTime { get; set; }

        public int TemperatureUnit { get; set; }

        public int MoonPhase { get; set; }

        public float Humidity { get; set; }

        public double AirPressureInHPA { get; set; }

        public DateTime SunRiseTime { get; set; }

        public DateTime SunSetTime { get; set; }

        public int UVIndex { get; set; }

        public int VisibilityInKM { get; set; }

        public int WindBearing { get; set; }

        public double WindSpeedInMS { get; set; }

        public float CloudCover { get; set; }
    }
}
