using System;
using System.Globalization;
using Xamarin.Forms;

namespace StarMap.Converters
{
  // Turns out it was not needed. Leave it here as a future reference.
  // In xaml:
  /*
   * xmlns:conv="clr-namespace:StarMap.Converters"
   * <!--<Slider.Minimum>
          <Binding Path="Minimum">
            <Binding.Converter>
              <conv:StringToDoubleConverter/>
            </Binding.Converter>
          </Binding>
        </Slider.Minimum>-->
   */
  public class StringToDoubleConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      double.TryParse(value.ToString(), out double res);
      return res;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
