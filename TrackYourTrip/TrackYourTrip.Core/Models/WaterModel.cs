using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.Models
{
    [SQLite.Table(TableConsts.WATERMODEL_TABLE)]
    public class WaterModel : IModel
    {
        public WaterModel()
        {
            this.Id = Guid.NewGuid();
            this.IsNew = false;
            this.IsValid = true;
        }

        [SQLite.PrimaryKey]
        public Guid Id { get; set; }
        public string Water { get; set; }

        [Ignore]
        public bool IsNew { get; set; }

        [Ignore]
        public bool IsValid { get; set; }
    }
}
