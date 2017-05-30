using StarMap.Models.ThreeDee;
using System;
using Urho.Forms;
using Xamarin.Forms;

namespace StarMap.Views
{
  public partial class StarDetailPage : ContentPage
  {
    public StarDetailPage()
    {
      InitializeComponent();
    }

    protected override async void OnAppearing()
    {
      var options = new Urho.ApplicationOptions(assetsFolder: "Data")
      {
        //Orientation = Urho.ApplicationOptions.OrientationType.LandscapeAndPortrait,
        // iOS only - which is a shame, because now I have to ensure the view height < width
        // from https://github.com/xamarin/urho/blob/master/Urho3D/Urho3D_Android/Sdl/SDLSurface.java
        // if (requestedOrientation == ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE) 
        //  if (mWidth < mHeight) skip = true;
        // and with skip=true nothing happens, with log Log.v("SDL", "Skip .. Surface is not ready.");        
      };
      await surface.Show<Detail>(options);
    }

    protected override void OnDisappearing()
    {
      UrhoSurface.OnDestroy();
      base.OnDisappearing();
    }

    //protected override bool OnBackButtonPressed()
    //{
    //  var a = (ViewModels.Core.Navigator)BindingContext;
    //  a.NavigateTest(new Uri("ms-app:///MasterDetail/MainPage", UriKind.Absolute));
    //  return true;
    //}
  }
}
