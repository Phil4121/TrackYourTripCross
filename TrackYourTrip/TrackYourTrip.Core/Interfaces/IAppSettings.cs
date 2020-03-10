// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Core.Interfaces
{
    public interface IAppSettings
    {
        string DarkSkySerial { get; }
        int DefaultThreadWaitTime { get; }
        string TripIdInProcess { get; set; }
        PreDefinedSpotSettings PreDefinedSpotSettings { get; set; }
    }
}