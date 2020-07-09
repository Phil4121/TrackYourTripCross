using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Core.Interfaces
{
    public interface IWeatherService
    {
        Task<bool> ServiceIsReachable(int maxTimeoutInMilliseconds);

        Task<WeatherTaskResponseModel> GetWeatherData(WeatherTaskRequestModel request, bool testIsReachable = false);
    }
}
