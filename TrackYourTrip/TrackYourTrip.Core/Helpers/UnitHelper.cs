using System;
using System.Collections.Generic;
using System.Text;
using TrackYourTrip.Core.Services;

namespace TrackYourTrip.Core.Helpers
{
    public static class UnitHelper
    {

        public static double GetConvertedTemperature(double temperatrueInCelcius)
        {
            int tempUnit = GenerallSettingsHelper.GetDefaultTemperatureUnit();

            if (tempUnit == (int)EnumHelper.TemperatureUnitEnum.C)
                return temperatrueInCelcius;

            return ConvertCelciusToFarenheit(temperatrueInCelcius);
        }

        static double ConvertCelciusToFarenheit(double temperatureInCelcius)
        {
            return temperatureInCelcius * (9 / 5) + 32; // temp in farenheit
        }
    }
}
