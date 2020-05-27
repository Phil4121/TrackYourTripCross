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
            Initialize();
        }

        public FishedSpotModel(bool isNew = false)
        {
            Initialize();
            Id = Guid.NewGuid();
            IsNew = isNew;
        }

        private void Initialize()
        {
            
        }


        [ForeignKey(typeof(TripModel)), NotNull]
        public Guid ID_Trip { get; set; }

        public Guid ID_Spot { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }


        [Ignore]
        public TripModel Trip { get; set; }

        [Ignore]
        public SpotModel Spot { get; set; }
    }
}
