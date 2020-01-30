using System;
using Xamarin.Forms;

namespace TrackYourTrip.Core.Converters
{
    public class DatetimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            DateTime datetime = (DateTime)value;

            return datetime.ToLocalTime().ToString("dd.MM.yyyy / HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
