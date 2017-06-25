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
    Stars a;
    public UrhoPage()
    {
      Urho.Application.UnhandledException += Application_UnhandledException;
      InitializeComponent();
    }
    private async void Application_UnhandledException(object sender, Urho.UnhandledExceptionEventArgs e)
    {
      await a?.Exit();
    }
    protected override async void OnAppearing()
    {
      
      a  = await surface.Show<Stars>(new Urho.ApplicationOptions("Data"));
    }

    

    protected override void OnDisappearing()
    {
      UrhoSurface.OnDestroy();
      base.OnDisappearing();
    }
  }
}