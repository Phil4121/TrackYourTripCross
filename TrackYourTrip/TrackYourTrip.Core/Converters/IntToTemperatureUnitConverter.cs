using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace TrackYourTrip.Core.Converters
{
    public class IntToTemperatureUnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;


            int unit;
            int.TryParse(value.ToString(), out unit);

            if (unit < 0)
                return string.Empty;

            switch(unit)
            {
                case 1:
                    return "°C";

                case 0:
                    return "°F";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
