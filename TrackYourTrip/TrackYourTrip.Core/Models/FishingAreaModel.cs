using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Helpers;
using Xamarin.Essentials;
using SQLiteNetExtensions.Attributes;
using TrackYourTrip.Core.Interfaces;
using SQLite;

namespace TrackYourTrip.Core.Models
{
    [SQLite.Table(TableConsts.FISHINGAREA_TABLE)]
    public class FishingAreaModel : IModel
    {
        public FishingAreaModel()
        {
            this.Id = Guid.NewGuid();
            this.IsNew = false;
            this.IsValid = false;
            Spots = new List<SpotModel>();
        }

        public FishingAreaModel(bool isNew = false)
        {
            this.Id = Guid.NewGuid();
            this.IsNew = isNew;
            this.IsValid = false;
            Spots = new List<SpotModel>();
        }

        [SQLite.PrimaryKey]
        public Guid Id { get; set; }
        public String FishingArea { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        [ForeignKey(typeof(WaterModel))]
        public Guid ID_WaterModel { get; set; }

        [OneToOne]
        public WaterModel WaterModel { get; set; }


        [OneToMany]
        public List<SpotModel> Spots { get; set; }

        public Location AreaLocation
        {
            get { return new Location(Lat, Lng); }
        }

        [Ignore]
        public bool IsNew { get; set; }

        [Ignore]
        public bool IsValid { get; set; }
    }
}
