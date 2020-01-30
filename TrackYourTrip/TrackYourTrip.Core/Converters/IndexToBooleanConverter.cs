using System;
using System.Globalization;
using Xamarin.Forms;

namespace TrackYourTrip.Core.Converters
{
    public class IndexToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            int.TryParse(value.ToString(), out int index);

            return index > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
