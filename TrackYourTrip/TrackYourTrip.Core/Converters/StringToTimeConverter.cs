using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace TrackYourTrip.Core.Converters
{
    public class StringToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "00:00";
            }

            DateTime dateTimeValue = new DateTime();
            DateTime.TryParse(value.ToString(), out dateTimeValue);

            return dateTimeValue.TimeOfDay.ToString().Substring(0,5);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tmp = ParseUserInputToTime(value.ToString());

            return new DateTime(1900, 01, 01, int.Parse(tmp.Substring(0, 2)), int.Parse(tmp.Substring(3, 2)), 0);
        }

        private string ParseUserInputToTime(string userInput)
        {
            if(TimeSpan.TryParse(userInput, out var formatedTime))
                return formatedTime.ToString();

            return "00:00";
        }
    }
}
