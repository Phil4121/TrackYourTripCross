using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Interfaces;
using Xamarin.Forms;

namespace TrackYourTrip.Core.Services
{
    public class DependencyServiceWrapper : IDependencyService
    {
        public T Get<T>() where T : class
        {
            return DependencyService.Get<T>();
        }
    }
}
