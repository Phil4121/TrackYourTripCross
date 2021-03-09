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
                Weather = new FishedSpotWeatherModel(isNew);
                Water = new FishedSpotWaterModel(isNew);
                Bites = new List<FishedSpotBiteModel>();
                Catches = new List<FishedSpotCatchModel>();
            }
        }


        [ForeignKey(typeof(TripModel)), NotNull]
        public Guid ID_Trip { get; set; }

        public Guid ID_FishingArea { get; set; }

        [ForeignKey(typeof(SpotModel)), NotNull]
        public Guid ID_Spot { get; set; }

        public Guid ID_FishedSpotWeather { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        [Ignore]
        public int BiteCount
        {
            get
            {
                if (Bites == null)
                    return 0;

                return Bites.Count;
            }
        }

        [Ignore]
        public int CatchCount
        {
            get
            {
                if (Catches == null)
                    return 0;

                return Catches.Count;
            }
        }


        [Ignore]
        public TripModel Trip { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public SpotModel Spot { get; set; }

        [Ignore]
        public FishingAreaModel FishingArea { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public FishedSpotWeatherModel Weather { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public FishedSpotWaterModel Water { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<FishedSpotBiteModel> Bites { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<FishedSpotCatchModel> Catches { get; set; }
    }
}
