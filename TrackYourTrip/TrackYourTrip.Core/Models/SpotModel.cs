using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.Models
{
    [SQLite.Table(TableConsts.SPOT_TABLE)]
    public class SpotModel : IModel
    {
        public SpotModel()
        {
            this.Id = Guid.NewGuid();
            this.IsNew = false;
        }

        public SpotModel(bool isNew = false)
        {
            this.Id = Guid.NewGuid();
            this.IsNew = isNew;
        }

        [SQLite.PrimaryKey]
        public Guid Id { get; set; }

        public string Spot { get; set; }

        [ForeignKey(typeof(FishingAreaModel))]
        public Guid ID_FishingArea { get; set; }

        [ForeignKey(typeof(SpotTypeModel))]
        public Guid ID_SpotType { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        [ManyToOne]
        public FishingAreaModel FishingArea { get; set; }

        [OneToOne]
        public SpotTypeModel SpotType { get; set; }

        [Ignore]
        public bool IsNew { get; set; }

        [Ignore]
        public bool IsValid { get; set; }
    }
}
