using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [SQLite.Table(TableConsts.WATERMODEL_TABLE)]
    public class WaterModel : BaseModel
    {
        public string Water { get; set; }
    }
}
