using System;
using System.Text;
using System.Collections.Generic;
using Xunit;
using TrackYourTrip.Core.Services.Wheater.DarkSky;
using System.Globalization;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.UnitTests
{
    public class WheaterServiceTest
    {
        public IWheaterService service = new DarkSkyWheaterService();

        public WheaterServiceTest()
        {

        }

        [Fact]
        public void TestServiceCall()
        {
            bool isReachable = service.ServiceIsReachable(3000).Result;
            Assert.True(isReachable,"Service is not reachable");
        }
    }
}
