using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.FISHEDSPOTWEATHER_TABLE)]
    public class FishedSpotWeatherModel : BaseModel
    {
        public FishedSpotWeatherModel()
        {
        }

        public FishedSpotWeatherModel(bool isNew = false)
        {
            Id = Guid.NewGuid();
            IsNew = isNew;
        }

        [ForeignKey(typeof(TripModel)), NotNull]
        public Guid ID_FishedSpot { get; set; }

        public bool IsOverwritten { get; set; }

        public int WeatherSituation { get; set; }

        public double CurrentTemperature { get; set; }

        public double DailyTemperatureHigh { get; set; }

        public DateTime DailyTemperatureHighTime { get; set; }

        public double DailyTemperatureLow { get; set; }

        public DateTime DailyTemperatureLowTime { get; set; }

        public int TemperatureUnit { get; set; }

        public double MoonPhase { get; set; }

        public double Humidity { get; set; }

        public double AirPressureInHPA { get; set; }

        public DateTime SunRiseTime { get; set; }

        public DateTime SunSetTime { get; set; }

        public int UVIndex { get; set; }

        public double VisibilityInKM { get; set; }

        public int WindBearing { get; set; }

        public double WindSpeedInMS { get; set; }

        public double CloudCover { get; set; }
    }
}
