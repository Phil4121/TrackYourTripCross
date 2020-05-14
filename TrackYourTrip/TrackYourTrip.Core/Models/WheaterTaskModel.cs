using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TrackYourTrip.Core.Models
{
    public class WheaterTaskModel
    {
        public DateTime RequestDateTime { get; set; }

        public double RequestLat { get; set; }

        public double RequestLng { get; set; }
    }
}
