using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.TRIP_TABLE)]
    public class TripModel : BaseModel
    {
        public TripModel()
        {
            Initialize();
        }

        public TripModel(bool isNew = false)
        {
            Initialize();
            Id = Guid.NewGuid();
            IsNew = isNew;
        }

        private void Initialize()
        {

        }

        [ForeignKey(typeof(FishingAreaModel)), NotNull]
        public Guid ID_FishingArea { get; set; }

        [OneToOne]
        public FishingAreaModel FishingArea { get; set; }

        public DateTime TripDateTime { get; set; }
    }
}
