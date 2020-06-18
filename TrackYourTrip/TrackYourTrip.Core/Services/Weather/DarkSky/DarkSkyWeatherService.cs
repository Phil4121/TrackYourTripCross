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
                }).Result.Success);

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

        public async Task<WeatherTaskResponseModel> GetWeatherData(WeatherTaskRequestModel request)
        {
            try
            {
                var response = new WeatherTaskResponseModel();

                var param = BuildOptionalParameters(request.CultureInfo);

                var forecast = await WeatherService.GetForecast(request.Lat, request.Lng, param);

                if (forecast?.IsSuccessStatus == true)
                {
                    response.Success = (bool)forecast?.IsSuccessStatus;
                    response.Temperature = SetTemperature((double)forecast.Response.Currently.Temperature);
                    response.TemperatureUnit = GetTemperatureUnit();
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

        private int GetTemperatureUnit(){
            return GenerallSettingsHelper.GetDefaultTemperatureUnit();
        }
    }
}
