using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UhroTest
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();
      butt.Clicked += (s, e) => Navigation.PushAsync(new UrhoPage());
    }
  }
}
