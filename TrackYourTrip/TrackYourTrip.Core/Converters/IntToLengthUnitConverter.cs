using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace TrackYourTrip.Core.Converters
{
    public class IntToLengthUnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;


            int unit;
            int.TryParse(value.ToString(), out unit);

            if (unit < 0)
                return string.Empty;

            if (parameter != null && 
                (parameter.ToString().ToLower() == "m" ||
                parameter.ToString().ToLower() == "i"))
            {
                switch (unit)
                {
                    case 1:
                        return "m";

                    case 0:
                        return "inch";
                }
            }
            else
            {
                switch (unit)
                {
                    case 1:
                        return "cm";

                    case 0:
                        return "foot";
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null &&
                (parameter.ToString().ToLower() == "m" ||
                parameter.ToString().ToLower() == "i"))
            {
                switch (value)
                {
                    case 'm':
                        return 1;

                    case "inch":
                        return 0;
                }
            }
            else
            {
                switch (value)
                {
                    case "cm":
                        return 1;

                    case "foot":
                        return 0;
                }
            }

            return -1;
        }
    }
}
