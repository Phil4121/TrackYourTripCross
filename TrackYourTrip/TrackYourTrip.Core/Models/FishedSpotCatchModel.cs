using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.FISHEDSPOTCATCH_TABLE)]
    public class FishedSpotCatchModel : BaseModel
    {
        public FishedSpotCatchModel()
        {

        }

        public FishedSpotCatchModel(bool isNew = false)
        {
            Id = Guid.NewGuid();
            IsNew = isNew;
        }

        [ForeignKey(typeof(TripModel)), NotNull]
        public Guid ID_Trip { get; set; }

        [ForeignKey(typeof(FishedSpotModel)), NotNull]
        public Guid ID_FishedSpot { get; set; }

        [ForeignKey(typeof(FishingAreaModel)), NotNull]
        public Guid ID_FishingArea { get; set; }

        public DateTime CatchDateTime { get; set; }

        [ForeignKey(typeof(FishingAreaModel))]
        public Guid ID_BiteDistance { get; set; }

        [ForeignKey(typeof(BaitTypeModel))]
        public Guid ID_BaitType { get; set; }

        [ForeignKey(typeof(BaitColorModel))]
        public Guid ID_BaitColor { get; set; }

        [ForeignKey(typeof(FishModel))]
        public Guid ID_Fish { get; set; }

        public int FishLength { get; set; }

        public int LengthUnit { get; set; }

        [Ignore]
        public BiteDistanceModel BiteDistance { get; set; }

        [Ignore]
        public BaitTypeModel BaitType { get; set; }

        [Ignore]
        public BaitColorModel BaitColor { get; set; }

        [Ignore]
        public FishModel Fish { get; set; }

    }
}
