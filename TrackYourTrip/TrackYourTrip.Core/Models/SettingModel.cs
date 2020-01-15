using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Models
{
    [SQLite.Table(TableConsts.SETTINGS_TABLE)]
    public class SettingModel : IModel
    {
        public SettingModel()
        {
            this.Id = Guid.NewGuid();
            this.IsNew = false;
            this.IsValid = false;
        }

        [SQLite.PrimaryKey]
        public Guid Id { get; set; }
        public string Setting { get; set; }
        public string LandingPage { get; set; }
        public int SortOrder { get; set; }
        
        [Ignore]
        public bool IsNew { get; set; }

        [Ignore]
        public bool IsValid { get; set; }
    }
}
