using StarMap.Cll.Models.Core;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace StarMap.Converters
{

  public class LocationToBooleanConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      => value != null && ((IReferencable)value).Designation != "Earth";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
