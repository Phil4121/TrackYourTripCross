using System;
using System.Text;
using System.Collections.Generic;
using Xunit;
using TrackYourTrip.Core.Services.Weather.DarkSky;
using System.Globalization;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Services.Weather;

namespace TrackYourTrip.UnitTests
{
    public class WheaterServiceTest
    {
        public IWeatherService service = new DarkSkyWeatherService();

        public WheaterServiceTest()
        {

        }

        [Fact]
        public void TestServiceCall()
        {
            bool isReachable = service.ServiceIsReachable(10000).Result;
            Assert.True(isReachable,"Service is not reachable");
        }
    }
}
