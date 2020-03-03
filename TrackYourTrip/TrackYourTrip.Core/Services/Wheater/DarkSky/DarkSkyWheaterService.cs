using DarkSky.Models;
using DarkSky.Services;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.Services.Wheater.DarkSky
{
    public class DarkSkyWheaterService : IWheaterService
    {
        private const string GERMAN_DEFAULT_UNITS = "si";
        private const string ENGLISH_DEFAULT_UNITS = "us";
        private const string SELECT_BY_COUNTRY = "auto";

        // for testing - Position of Altenfelden, Upper Austria
        private const double TEST_LAT = 48.4868;
        private const double TEST_LNG = 13.9662;

        string Serial { get; set; }

        DarkSkyService WheaterService { get; set; }

        public DarkSkyWheaterService()
        {
            Serial = new GlobalSettings().DarkSkySerial;
            WheaterService = new DarkSkyService(Serial);
        }

        public async Task<bool> ServiceIsReachable(int maxTimeoutInMilliseconds)
        {
            try
            {
                bool isReachable = false;

                CancellationTokenSource cts = new CancellationTokenSource();

                Task<bool> task = new Task<bool>(() => QueryService(TEST_LAT, TEST_LNG).Result.IsSuccessStatus);

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



        private async Task<DarkSkyResponse> QueryService(double lat, double lng, OptionalParameters parameters = null)
        {
            return await WheaterService.GetForecast(lat, lng, parameters);
        }


        public async Task GetWheaterData(double lat, double lng, CultureInfo cultureInfo)
        {
            try
            {
                var param = BuildOptionalParameters(cultureInfo);

                var forecast = WheaterService.GetForecast(lat, lng, param).Result;

                if (forecast?.IsSuccessStatus == true)
                {
                    Console.WriteLine(forecast.Response.Currently.Summary);
                }
                else
                {
                    Console.WriteLine("No current weather data");
                }
                Console.WriteLine(forecast.AttributionLine);
                Console.WriteLine(forecast.DataSource);

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
