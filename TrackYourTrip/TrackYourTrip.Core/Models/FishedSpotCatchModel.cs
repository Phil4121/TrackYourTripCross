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
    }
}
