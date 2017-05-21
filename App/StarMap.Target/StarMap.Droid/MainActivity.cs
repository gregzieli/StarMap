using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Views;
using StarMap.Droid.Database;

namespace StarMap.Droid
{
  [Activity(Label = "@string/app_name", 
    Theme = "@style/MyTheme", 
    ScreenOrientation = ScreenOrientation.Landscape,
    MainLauncher = true, 
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
  {
    protected override void OnCreate(Bundle bundle)
    {
      TabLayoutResource = Resource.Layout.Tabbar;
      ToolbarResource = Resource.Layout.Toolbar;

      //Remove notification bar
      Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

      base.OnCreate(bundle);

      Connector.CheckDatabase();

      Xamarin.Forms.Forms.Init(this, bundle);

      LoadApplication(new App());
    }
  }
}

