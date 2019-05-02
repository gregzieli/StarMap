using StarMap.Core.Abstractions;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace StarMap.Converters
{
    public class LocationToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
          => value != null && ((IUnique)value).Id != 0;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
