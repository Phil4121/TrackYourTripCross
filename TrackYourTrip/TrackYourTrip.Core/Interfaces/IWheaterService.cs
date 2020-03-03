using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrackYourTrip.Core.Interfaces
{
    public interface IWheaterService
    {
        Task<bool> ServiceIsReachable(int maxTimeoutInMilliseconds);
    }
}
