using SQLite;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.FISH_TABLE)]
    public class FishModel : BaseModel
    {
        public string FishName { get; set; }
    }
}
