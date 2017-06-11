using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UhroTest
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class UrhoPage : ContentPage
  {
    public UrhoPage()
    {
      InitializeComponent();
    }

    protected override async void OnAppearing()
    {
      await surface.Show<Stars>(new Urho.ApplicationOptions());
    }

    protected override void OnDisappearing()
    {
      UrhoSurface.OnDestroy();
      base.OnDisappearing();
    }
  }
}