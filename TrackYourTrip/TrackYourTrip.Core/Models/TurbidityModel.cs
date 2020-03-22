using SQLite;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.TURBIDITY_TABLE)]
    public class TurbidityModel : BaseModel
    {
        public string Turbidity { get; set; }
    }
}
