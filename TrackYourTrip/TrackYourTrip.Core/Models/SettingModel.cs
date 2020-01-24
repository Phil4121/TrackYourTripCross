using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Models
{
    [SQLite.Table(TableConsts.SETTINGS_TABLE)]
    public class SettingModel : BaseModel
    {
        public string Setting { get; set; }
        public string LandingPage { get; set; }
        public int SortOrder { get; set; }
        
    }
}
