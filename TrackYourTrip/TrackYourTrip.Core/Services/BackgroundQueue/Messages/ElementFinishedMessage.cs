using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Core.Services.BackgroundQueue.Messages
{
    public class ElementFinishedMessage
    {
        public BackgroundTaskModel BackgroundTask {get; set;}
    }
}
