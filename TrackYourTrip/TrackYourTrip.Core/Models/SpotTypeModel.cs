using SQLite;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.SPOTTYPE_TABLE)]
    public class SpotTypeModel : BaseModel
    {
        public string SpotType { get; set; }
    }
}
