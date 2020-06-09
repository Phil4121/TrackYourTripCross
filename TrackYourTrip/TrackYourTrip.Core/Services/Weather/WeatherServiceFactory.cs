using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Services.Weather.DarkSky;

namespace TrackYourTrip.Core.Services.Weather
{
    public static class WeatherServiceFactory
    {
        public static IWeatherService GetWeatherServiceFactory()
        {
            return new DarkSkyWeatherService();
        }
    }
}
