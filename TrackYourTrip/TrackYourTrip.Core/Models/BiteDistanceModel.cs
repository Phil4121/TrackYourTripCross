using SQLite;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.BITEDISTANCE_TABLE)]
    public class BiteDistanceModel : BaseModel
    {
        public string BiteDistance { get; set; }

    }
}
