using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Core.Interfaces
{
    public interface ISharedFishedSpotViewModel
    {
        FishedSpotModel FishedSpot { get; set; }
    }
}
