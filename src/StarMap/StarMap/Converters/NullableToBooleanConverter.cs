using System;
using System.Globalization;
using Xamarin.Forms;

namespace StarMap.Converters
{

    public class NullableToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? (object)false : value is string s ? s != string.Empty : (object)true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
