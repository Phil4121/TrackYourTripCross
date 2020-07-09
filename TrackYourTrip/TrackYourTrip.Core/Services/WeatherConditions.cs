using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Resources;

namespace TrackYourTrip.Core.Services
{
    public class WeatherConditions
    {
        public enum PossibleWeatherConditionEnum : int {
            WeatherClearDay = 1,
            WeatherClearNight = 2,
            WeatherRain = 3,
            WeatherSnow = 4,
            WeatherSleet = 5,
            WeatherWind = 6,
            WeatherFog = 7,
            WeatherCloudy = 8,
            WeatherPartlyCloudyDay = 9,
            WeatherPartlyCloudyNight = 10,
            NoData = 99
        };

        public ILocalizeService LocalizeService { get; private set; }

        public WeatherConditions(ILocalizeService localizeService)
        {
            this.LocalizeService = localizeService;
        }

        public IEnumerable<KeyValueModel> GetWeatherConditions()
        {
            var conditions = new List<KeyValueModel>();


            foreach(PossibleWeatherConditionEnum c in (PossibleWeatherConditionEnum[])Enum.GetValues(typeof(PossibleWeatherConditionEnum)))
            {
                conditions.Add(new KeyValueModel() { 
                    Key = (int)(PossibleWeatherConditionEnum)Enum.Parse(typeof(PossibleWeatherConditionEnum), c.ToString()),
                    Value = LocalizeService.Translate(c.ToString()) }); ;
            }

            return conditions;
        }
    }
}
