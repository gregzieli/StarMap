using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UhroTest
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ColorPage : ContentPage
  {
    public ColorPage()
    {
      InitializeComponent();

      StackLayout stackLayout = new StackLayout();
      var list = new List<Label>();

      foreach (FieldInfo info in typeof(Color).GetRuntimeFields())
      {
        // Skip the obsolete (i.e. misspelled) colors. 
        if (info.GetCustomAttribute<ObsoleteAttribute>() != null)
          continue;

        if (info.IsPublic && info.IsStatic && info.FieldType == typeof(Color))
          list.Add(CreateColorLabel((Color)info.GetValue(null), info.Name));
      }

      foreach (PropertyInfo info in typeof(Color).GetRuntimeProperties())
      {
        MethodInfo methodInfo = info.GetMethod;
        if (methodInfo.IsPublic && methodInfo.IsStatic && methodInfo.ReturnType == typeof(Color))
        {
          list.Add(CreateColorLabel((Color)info.GetValue(null), info.Name));
        }
      }

      Padding = new Thickness(5, Device.OnPlatform(20, 5, 5), 5, 5);

      list.Sort(delegate (Label left, Label right)
      {
        return left.BackgroundColor.Hue.CompareTo(right.BackgroundColor.Hue);
      });

      foreach (var item in Colors)
      {
        item.MinimumHeightRequest = 50;
        item.HeightRequest = 50;
        stackLayout.Children.Add(item);
      }
      

      // Put the StackLayout in a ScrollView. 
      Content = new ScrollView { Content = stackLayout };
    }

    List<Label> Colors = new List<Label>
      {
        new Label { BackgroundColor = Color.DodgerBlue, Text = "DodgerBlue" },
        new Label { BackgroundColor = Color.DeepSkyBlue, Text = "DeepSkyBlue" },
        new Label { BackgroundColor = Color.LightSkyBlue, Text = "LightSkyBlue" },
        new Label { BackgroundColor = Color.AliceBlue, Text = "AliceBlue" },
        new Label { BackgroundColor = Color.White, Text = "White" },
        new Label { BackgroundColor = Color.Ivory, Text = "Ivory" },
        new Label { BackgroundColor = Color.LightYellow, Text = "LightYellow" },
        new Label { BackgroundColor = Color.LemonChiffon, Text = "LemonChiffon" },
        new Label { BackgroundColor = Color.Yellow, Text = "Yellow" },
        new Label { BackgroundColor = Color.Gold, Text = "Gold" },
        new Label { BackgroundColor = Color.Orange, Text = "Orange" },
        new Label { BackgroundColor = Color.DarkOrange, Text = "DarkOrange" },
        new Label { BackgroundColor = Color.OrangeRed, Text = "OrangeRed" },
        new Label { BackgroundColor = Color.Tomato, Text = "Tomato" },
        new Label { BackgroundColor = Color.Red, Text = "Red" },
        new Label { BackgroundColor = Color.Firebrick, Text = "Firebrick" }
      };

    Label CreateColorLabel(Color color, string name)
    {
      Color backgroundColor = Color.Default;
      if (color != Color.Default)
      {
        // Standard luminance calculation. 
        double luminance = 0.30 * color.R + 0.59 * color.G + 0.11 * color.B;
        backgroundColor = luminance > 0.5 ? Color.Black : Color.White;
      }
      // Create the Label. 
      return new Label
      {
        Text = name,
        TextColor = backgroundColor,
        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
        BackgroundColor = color,
        HeightRequest = 40
      };
    }
  }
}