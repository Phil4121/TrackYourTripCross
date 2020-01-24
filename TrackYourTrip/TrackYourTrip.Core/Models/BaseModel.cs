using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.Models
{
    public abstract class BaseModel : IModel
    {
        public BaseModel()
        {
            this.IsNew = false;
            this.IsValid = false;
        }

        [PrimaryKey]
        public Guid Id { get; set; }

        [Ignore]
        public bool IsNew { get; set; }

        [Ignore]
        public bool IsValid { get; set; }
    }
}
