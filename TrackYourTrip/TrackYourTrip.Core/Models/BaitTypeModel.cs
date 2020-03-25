using SQLite;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.BAITTYPE_TABLE)]
    public class BaitTypeModel : BaseModel
    {
        public string BaitType { get; set; }
    }
}
