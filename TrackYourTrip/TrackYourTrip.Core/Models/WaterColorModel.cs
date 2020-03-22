using SQLite;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.WATERCOLOR_TABLE)]
    public class WaterColorModel : BaseModel
    {
        public string WaterColor { get; set; }
    }
}
