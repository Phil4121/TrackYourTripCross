using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Services.Wheater.DarkSky;
using Xunit;

namespace TrackYourTrip.UnitTests
{
    public class WheaterServiceTests
    {
        [Fact]
        public void TestWheaterService()
        {
            var srv = new DarkSkyWheaterService();

            srv.GetWheaterData();
        }
    }
}
