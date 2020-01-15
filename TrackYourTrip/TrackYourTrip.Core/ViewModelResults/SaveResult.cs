using System;
using System.Collections.Generic;
using System.Text;

namespace TrackYourTrip.Core.ViewModelResults
{
    public class SaveResult<TEntity>
    {
        public TEntity Entity { get; set; }
        public bool Saved { get; set; }
    }
}
