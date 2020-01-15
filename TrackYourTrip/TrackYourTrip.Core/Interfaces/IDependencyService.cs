using System;
using System.Collections.Generic;
using System.Text;

namespace TrackYourTrip.Core.Interfaces
{
    public interface IDependencyService
    {
        T Get<T>() where T : class;
    }
}
