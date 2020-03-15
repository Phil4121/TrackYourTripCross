using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.GENERALL_SETTING_TABLE)]
    public class GenerallSettingModel : BaseModel
    {
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        public int SortOrder { get; set; }
    }
}
