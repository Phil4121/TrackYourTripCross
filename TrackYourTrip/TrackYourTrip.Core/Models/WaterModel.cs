using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.Models
{
    [SQLite.Table(TableConsts.WATERMODEL_TABLE)]
    public class WaterModel : BaseModel
    {
        public string Water { get; set; }
    }
}
