using SQLite;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.CURRENT_TABLE)]
    public class CurrentModel : BaseModel
    {
        public string Current { get; set; }
    }
}
