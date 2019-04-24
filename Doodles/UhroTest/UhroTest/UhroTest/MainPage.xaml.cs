using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Reflection;

namespace UhroTest
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();
      butt.Clicked += (s, e) => Navigation.PushAsync(new UrhoPage());
      colorButt.Clicked += (s, e) => Navigation.PushAsync(new ColorPage());
    }
  }
}
