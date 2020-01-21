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

        [PrimaryKey]
        public Guid Id { get; set; }

        public string Spot { get; set; }

        [ForeignKey(typeof(FishingAreaModel)), NotNull]
        public Guid ID_FishingArea { get; set; }

        [ForeignKey(typeof(SpotTypeModel)), NotNull]
        public Guid ID_SpotType { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public FishingAreaModel FishingArea { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public SpotTypeModel SpotType { get; set; }

        [Ignore]
        public bool IsNew { get; set; }

        [Ignore]
        public bool IsValid { get; set; }
    }
}
