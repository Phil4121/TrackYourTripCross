using SQLite;
using System;
using TrackYourTrip.Core.Helpers;

namespace TrackYourTrip.Core.Models
{
    [Table(TableConsts.BACKGROUND_TASK_TABLE)]
    public class BackgroundTaskModel : BaseModel
    {
        public BackgroundTaskModel()
        {
            Id = Guid.NewGuid();
        }

        public BackgroundTaskModel(bool isNew = false)
        {
            Id = Guid.NewGuid();
            IsNew = isNew;
        }

        public int ID_TaskType { get; set; }

        public Guid ID_ElementReference { get; set; }

        public DateTime CreationDateTime { get; set; }

        public string TaskData { get; set; }

        public string TaskResponse { get; set; }

        [Ignore]
        public bool ProcessedSuccessfully { get; set; }
    }
}
