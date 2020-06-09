using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    public class WeatherTaskRequestModel
    {
        public double Lat { get; set; }

        public double Lng { get; set; }

        public DateTime RequestDateTime { get; set; }

        public CultureInfo CultureInfo { get; set; }
    }
}
