using System;
using System.Globalization;
using Xamarin.Forms;

namespace StarMap.Converters
{

  public class NullableToBooleanConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return false;

      if (value is string s)
        return s != string.Empty;

      return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
