using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.SPOT_TABLE)]
    public class SpotModel : BaseModel
    {
        public SpotModel()
        {
            Initialize();
        }

        public SpotModel(bool isNew = false)
        {
            Initialize();
            this.Id = Guid.NewGuid();
            this.IsNew = isNew;
        }

        private void Initialize()
        {
            SpotMarker = new List<SpotMarkerModel>();
        }


        public string Spot { get; set; }

        [ForeignKey(typeof(FishingAreaModel)), NotNull]
        public Guid ID_FishingArea { get; set; }

        [ForeignKey(typeof(SpotTypeModel)), NotNull]
        public Guid ID_SpotType { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public SpotTypeModel SpotType { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<SpotMarkerModel> SpotMarker { get; set; }

        [Ignore]
        public FishingAreaModel FishingArea { get; set; }
    }
}
