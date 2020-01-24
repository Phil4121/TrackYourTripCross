using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.SPOTTYPE_TABLE)]
    public class SpotTypeModel : BaseModel
    {
        public string SpotType { get; set; }
    }
}
