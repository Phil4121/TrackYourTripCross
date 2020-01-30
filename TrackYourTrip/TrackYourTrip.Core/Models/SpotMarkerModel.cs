using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.SPOTMARKER_TABLE)]
    public class SpotMarkerModel : BaseModel
    {
        public SpotMarkerModel()
        {
        }

        public SpotMarkerModel(bool isNew = false)
        {
            Id = Guid.NewGuid();
            IsNew = isNew;
        }

        public string SpotMarker { get; set; }

        [ForeignKey(typeof(SpotModel)), NotNull]
        public Guid ID_Spot { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        [Ignore]
        public SpotModel Spot { get; set; }

    }
}
