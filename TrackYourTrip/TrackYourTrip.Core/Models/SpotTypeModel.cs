using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.Models
{
    [SQLite.Table(TableConsts.SPOTTYPE_TABLE)]
    public class SpotTypeModel : IModel
    {
        public SpotTypeModel()
        {
            this.Id = Guid.NewGuid();
            this.IsNew = false;
            this.IsValid = true;
        }

        [SQLite.PrimaryKey]
        public Guid Id { get; set; }
        public string SpotType { get; set; }

        [Ignore]
        public bool IsNew { get; set; }

        [Ignore]
        public bool IsValid { get; set; }
    }
}
