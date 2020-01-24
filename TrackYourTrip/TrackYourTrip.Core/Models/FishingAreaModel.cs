using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Helpers;
using Xamarin.Essentials;
using SQLiteNetExtensions.Attributes;
using TrackYourTrip.Core.Interfaces;
using SQLite;
using System.ComponentModel;

namespace TrackYourTrip.Core.Models
{
    [SQLite.Table(TableConsts.FISHINGAREA_TABLE)]
    public class FishingAreaModel : BaseModel
    {
        public FishingAreaModel()
        {
            Initialize();
        }

        public FishingAreaModel(bool isNew = false)
        {
            Initialize();
            this.Id = Guid.NewGuid();
            this.IsNew = isNew;
        }

        private void Initialize()
        {
            Spots = new List<SpotModel>();
        }

        public String FishingArea { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        [ForeignKey(typeof(WaterModel)), NotNull]
        public Guid ID_WaterModel { get; set; }

        [OneToOne]
        public WaterModel WaterModel { get; set; }


        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<SpotModel> Spots { get; set; }

        public Location AreaLocation
        {
            get { return new Location(Lat, Lng); }
        }
    }
}
