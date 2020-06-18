using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services;

namespace TrackYourTrip.Core.Helpers
{
    public static class GenerallSettingsHelper
    {
        static IEnumerable<GenerallSettingModel> _generallSettings = null;

        static IEnumerable<GenerallSettingModel> GenerallSettings
        {
            get
            {
                if (_generallSettings != null)
                    return _generallSettings;

                _generallSettings = DataServiceFactory.GetGenerallSettingFactory().GetItemsAsync().Result;

                return _generallSettings;
            }
        }

        public static int GetDefaultTemperatureUnit()
        {
            return Convert.ToInt32(GenerallSettings.Where(s => s.SettingKey == TableConsts.DEFAULT_TEMPERATURE_UNIT).First().SettingValue);
        }

        public static int GetDefaultLengthUnit()
        {
            return Convert.ToInt32(GenerallSettings.Where(s => s.SettingKey == TableConsts.DEFAULT_LENGTH_UNIT).First().SettingValue);
        }
    }
}
