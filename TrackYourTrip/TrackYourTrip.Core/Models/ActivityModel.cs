using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using static TrackYourTrip.Core.Helpers.EnumHelper;

namespace TrackYourTrip.Core.Models
{
    public class ActivityModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public TaskTypeEnum TaskType { get; set; }

        public string Description { get; set; }

        public DateTime ExecutionDateTime { get; set; }

        public string AdditionalText { get; set; }
    }
}
