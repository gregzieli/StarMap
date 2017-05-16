using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace StarMap.Controls
{
  public partial class SliderCell : ViewCell
  {
    const double _zero = default(double);
    static readonly Type _doubleType = typeof(double);
    static readonly Type _thisType = typeof(SliderCell);

    public static readonly BindableProperty LabelProperty = BindableProperty.Create("Label", typeof(string), _thisType, null);

    public static readonly BindableProperty MinimumProperty = BindableProperty.Create("Minimum", _doubleType, _thisType, _zero);

    public static readonly BindableProperty MaximumProperty = BindableProperty.Create("Maximum", _doubleType, _thisType, 100.0);

    public static readonly BindableProperty SelectedValueProperty = BindableProperty.Create("SelectedValue", _doubleType, _thisType, _zero, BindingMode.TwoWay,
      propertyChanged: (sender, oldValue, newValue) =>
      {
        // It seems that it's not really necessary, just need to be not null to work.
        //SliderCell slider = (SliderCell)sender;
        //slider.ValueChanged?.Invoke(slider, (double)newValue);
      });
    //public event EventHandler<double> ValueChanged;

    public string Label
    {
      get { return (string)GetValue(LabelProperty); }
      set { SetValue(LabelProperty, value); }
    }

    public double Minimum
    {
      get { return (double)GetValue(MinimumProperty); }
      set { SetValue(MinimumProperty, value); }
    }

    public double Maximum
    {
      get { return (double)GetValue(MaximumProperty); }
      set { SetValue(MaximumProperty, value); }
    }
    
    public double SelectedValue
    {
      get { return (double)GetValue(SelectedValueProperty); }
      set { SetValue(SelectedValueProperty, value); }
    }

    public SliderCell()
    {
      InitializeComponent();
    }
  }
}
