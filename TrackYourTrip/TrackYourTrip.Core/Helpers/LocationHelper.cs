using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace TrackYourTrip.Core.Helpers
{
    public class LocationHelper
    {
        public static Location GetCurrentLocation()
        {
            try
            {
                return Geolocation.GetLastKnownLocationAsync().Result;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                throw;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                throw;
            }
            catch (PermissionException pEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
