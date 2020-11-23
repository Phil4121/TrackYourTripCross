using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.FISHEDSPOT_TABLE)]
    public class FishedSpotModel : BaseModel
    {
        public FishedSpotModel()
        {

        }

        public FishedSpotModel(bool isNew = false)
        {
            Id = Guid.NewGuid();
            IsNew = isNew;

            if (isNew)
            {
                ID_FishedSpotWeather = Guid.NewGuid();
                Weather = new FishedSpotWeatherModel();
                Water = new FishedSpotWaterModel();
            }
        }


        [ForeignKey(typeof(TripModel)), NotNull]
        public Guid ID_Trip { get; set; }

        public Guid ID_FishingArea { get; set; }

        public Guid ID_Spot { get; set; }

        public Guid ID_FishedSpotWeather { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }


        [Ignore]
        public TripModel Trip { get; set; }

        [Ignore]
        public SpotModel Spot { get; set; }

        [Ignore]
        public FishingAreaModel FishingArea { get; set; }

        [Ignore]
        public FishedSpotWeatherModel Weather { get; set; }

        [Ignore]
        public FishedSpotWaterModel Water { get; set; }
    }
}
