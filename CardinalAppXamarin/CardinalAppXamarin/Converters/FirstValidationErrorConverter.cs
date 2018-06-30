using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace CardinalAppXamarin.Converters
{
    public class FirstValidationErrorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is List<string> ls
                && ls.Count > 0)
            {
                return ls[0];
            }
            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string vs)
            {
                return new List<string>() { vs };
            }
            return new List<string>();
        }
    }
}
