using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.FISHEDSPOTWEATHER_TABLE)]
    public class FishedSpotWeatherModel : BaseModel
    {
        public FishedSpotWeatherModel()
        {
        }

        public FishedSpotWeatherModel(bool isNew = false)
        {
            Id = Guid.NewGuid();
            IsNew = isNew;
        }

        [ForeignKey(typeof(TripModel)), NotNull]
        public Guid ID_FishedSpot { get; set; }

        public bool IsOverwritten { get; set; }

        public double Temperature { get; set; }

        public int WeatherSituation { get; set; }

    }
}
