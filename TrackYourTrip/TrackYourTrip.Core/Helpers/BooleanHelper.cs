using System;
using System.Collections.Generic;
using System.Text;

namespace TrackYourTrip.Core.Helpers
{
    public static class BooleanHelper
    {
        public static String ConvertBooltoString(bool value)
        {
            return value == true ? "1" : "0";
        }
    }
}
