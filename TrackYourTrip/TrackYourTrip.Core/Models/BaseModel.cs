using SQLite;
using System;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.Models
{
    public abstract class BaseModel : IModel
    {
        public BaseModel()
        {
            IsNew = false;
            IsValid = false;
        }

        [PrimaryKey]
        public Guid Id { get; set; }

        [Ignore]
        public bool IsNew { get; set; }

        [Ignore]
        public bool IsValid { get; set; }
    }
}
