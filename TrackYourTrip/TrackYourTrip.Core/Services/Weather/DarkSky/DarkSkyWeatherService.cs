using DarkSky.Models;
using DarkSky.Services;
using FluentValidation.Validators;
using MvvmCross;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Core.Services.Weather.DarkSky
{
    public class DarkSkyWeatherService : IWeatherService
    {
        private const string GERMAN_DEFAULT_UNITS = "si";
        private const string ENGLISH_DEFAULT_UNITS = "us";
        private const string SELECT_BY_COUNTRY = "auto";

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
            catch(Exception ex)
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
                    response.Temperature = (double) forecast.Response.Currently.Temperature;
                }
                else
                {
                    Console.WriteLine("No current weather data");
                }

                return response;

            }catch(Exception ex)
            {
                throw ex;
            }
        }



        private OptionalParameters BuildOptionalParameters(CultureInfo culture)
        {
            var param = new OptionalParameters();

            switch (culture.Name){
                case "de-DE":
                    param.MeasurementUnits = GERMAN_DEFAULT_UNITS;
                    break;

                default:
                    param.MeasurementUnits = SELECT_BY_COUNTRY;
                    break;
            }

            return param;
        }
    }
}
