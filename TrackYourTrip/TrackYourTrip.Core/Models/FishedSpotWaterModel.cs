using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.FISHEDSPOTWATER_TABLE)]
    public class FishedSpotWaterModel : BaseModel
    {
        public FishedSpotWaterModel()
        {
        }

        public FishedSpotWaterModel(bool isNew = false)
        {
            Id = Guid.NewGuid();
            IsNew = isNew;
        }

        [ForeignKey(typeof(TripModel)), NotNull]
        public Guid ID_FishedSpot { get; set; }

        public bool IsOverwritten { get; set; }

        public double WaterTemperature { get; set; }

        public int WaterTemperatureUnit { get; set; }

        public double WaterLevel { get; set; }

        public int WaterLevelUnit { get; set; }

        [ForeignKey(typeof(WaterColorModel)), NotNull]
        public Guid ID_WaterColor { get; set; }

        [ForeignKey(typeof(TurbidityModel)), NotNull]
        public Guid ID_Turbidity { get; set; }

        [ForeignKey(typeof(CurrentModel)), NotNull]
        public Guid ID_Current { get; set; }
    }
}
