using System;
using System.Globalization;

namespace CardinalAppXamarin.Converters
{
    public class TimeStampToLastUpdatedStringConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                var date = (DateTime)value;
                TimeSpan span = DateTime.Now.Subtract(date);
                return String.Format("{0}mins ago.", (int)span.TotalMinutes);
            }
            else
            {
                throw new InvalidOperationException("The target must be a DateTime");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
