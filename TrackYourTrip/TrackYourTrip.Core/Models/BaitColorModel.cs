using SQLite;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.BAITCOLOR_TABLE)]
    public class BaitColorModel : BaseModel
    {
        public string BaitColor { get; set; }
    }
}
