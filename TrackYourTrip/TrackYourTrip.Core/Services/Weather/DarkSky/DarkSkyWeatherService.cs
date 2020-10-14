using DarkSky.Models;
using DarkSky.Services;
using FluentValidation.Validators;
using MvvmCross;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using static TrackYourTrip.Core.Services.WeatherConditions;

namespace TrackYourTrip.Core.Services.Weather.DarkSky
{
    public class DarkSkyWeatherService : IWeatherService
    {
        /*SI Units: 
        summary: Any summaries containing temperature or snow accumulation units will have their values in degrees Celsius or in centimeters(respectively).
        nearestStormDistance: Kilometers.
        precipIntensity: Millimeters per hour.
        precipIntensityMax: Millimeters per hour.
        precipAccumulation: Centimeters.
        temperature: Degrees Celsius.
        temperatureMin: Degrees Celsius.
        temperatureMax: Degrees Celsius.
        apparentTemperature: Degrees Celsius.
        dewPoint: Degrees Celsius.
        windSpeed: Meters per second.
        pressure: Hectopascals.
        visibility: Kilometers. */

        private const string GERMAN_DEFAULT_UNITS = "si";

        // for testing - Position of Altenfelden, Upper Austria
        private const double TEST_LAT = 48.4868;
        private const double TEST_LNG = 13.9662;

        string Serial { get; set; }

        DarkSkyService WeatherService { get; set; }

        public DarkSkyWeatherService()
        {
            var settings = Mvx.IoCProvider.Resolve<IAppSettings>();

            Serial = settings.DarkSkySerial;
            WeatherService = new DarkSkyService(Serial);
        }

        public async Task<bool> ServiceIsReachable(int maxTimeoutInMilliseconds)
        {
            try
            {
                bool isReachable = false;

                CancellationTokenSource cts = new CancellationTokenSource();

                var request = new WeatherTaskRequestModel()
                {
                    Lat = TEST_LAT,
                    Lng = TEST_LNG,
                    CultureInfo = CultureInfo.CurrentCulture,
                    RequestDateTime = DateTime.Now
                };

                Task<bool> task = new Task<bool>(() => GetWeatherData(new WeatherTaskRequestModel()
                {
                    Lat = TEST_LAT,
                    Lng = TEST_LNG,
                    CultureInfo = CultureInfo.CurrentCulture
                }, true).Result.Success);

                task.Start();

                await Task.WhenAny(task, Task<bool>.Delay(maxTimeoutInMilliseconds, cts.Token));

                if (task.Status == TaskStatus.RanToCompletion)
                    isReachable = task.Result;

                return isReachable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<WeatherTaskResponseModel> GetWeatherData(WeatherTaskRequestModel request, bool testIsReachable = false)
        {
            try
            {
                var response = new WeatherTaskResponseModel();

                var param = BuildOptionalParameters(request.CultureInfo);

                var forecast = await WeatherService.GetForecast(request.Lat, request.Lng, param);

                if (forecast?.IsSuccessStatus == true)
                {
                    response.Success = (bool)forecast?.IsSuccessStatus;

                    if (testIsReachable)
                        return response;

                    response.WeatherSituation = ParseWeatherCondition(forecast.Response.Currently.Icon.ToString());
                    response.CurrentTemperature = SetTemperature((double)forecast.Response.Currently.Temperature);
                    response.TemperatureUnit = GetTemperatureUnit();
                    response.DailyTemperatureHigh = SetTemperature(forecast.Response.Daily.Data[0].TemperatureHigh.GetValueOrDefault());
                    response.DailyTemperatureHighTime = SetDateTime(forecast.Response.Daily.Data[0].TemperatureHighDateTime.GetValueOrDefault());
                    response.DailyTemperatureLow = SetTemperature((forecast.Response.Daily.Data[0].TemperatureLow.GetValueOrDefault()));
                    response.DailyTemperatureLowTime = SetDateTime(forecast.Response.Daily.Data[0].TemperatureLowDateTime.GetValueOrDefault());
                    response.MoonPhase = forecast.Response.Daily.Data[0].MoonPhase.GetValueOrDefault();
                    response.Humidity = forecast.Response.Currently.Humidity.GetValueOrDefault();
                    response.AirPressureInHPA = forecast.Response.Currently.Pressure.GetValueOrDefault();
                    response.SunRiseTime = SetDateTime(forecast.Response.Daily.Data[0].SunriseDateTime);
                    response.SunSetTime = SetDateTime(forecast.Response.Daily.Data[0].SunsetDateTime);
                    response.UVIndex = forecast.Response.Currently.UvIndex.GetValueOrDefault();
                    response.VisibilityInKM = forecast.Response.Currently.Visibility.GetValueOrDefault();
                    response.WindBearing = forecast.Response.Currently.WindBearing.GetValueOrDefault();
                    response.WindSpeedInMS = forecast.Response.Currently.WindSpeed.GetValueOrDefault();
                    response.CloudCover = forecast.Response.Currently.CloudCover.GetValueOrDefault();
                }
                else
                {
                    Console.WriteLine("No current weather data");
                }

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private OptionalParameters BuildOptionalParameters(CultureInfo culture)
        {
            var param = new OptionalParameters();

            // always use "SI" -> transform later if necessary
            param.MeasurementUnits = GERMAN_DEFAULT_UNITS;

            return param;
        }

        private double SetTemperature(double wsTemperature) {
            return UnitHelper.GetConvertedTemperature(wsTemperature);
        }

        private DateTime SetDateTime(DateTimeOffset? dateTimeOffset)
        {
            if (dateTimeOffset.Value == null)
                return new DateTime();

            return dateTimeOffset.Value.DateTime;
        }

        private int GetTemperatureUnit(){
            return GenerallSettingsHelper.GetDefaultTemperatureUnit();
        }

        private int ParseWeatherCondition(string weatherCondition)
        {
            var condition =  (int) PossibleWeatherConditionEnum.WeatherClearDay;

            if (condition < 1)
                return 99;

            return condition;
        }

    }
}
