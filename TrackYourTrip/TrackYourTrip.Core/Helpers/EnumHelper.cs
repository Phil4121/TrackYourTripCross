using System;
using System.Collections.Generic;
using System.Text;

namespace TrackYourTrip.Core.Helpers
{
    public static class EnumHelper
    {
        public enum TemperatureUnitEnum
        {
            C = 1,
            F = 0
        }

        public enum LengthUnitEnum
        {
            M = 1, // m/cm
            I = 0 // inch/foot
        }

        public enum TaskTypeEnum
        {
            WheaterTask = 1
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
