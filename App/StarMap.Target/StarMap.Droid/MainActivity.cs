using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using StarMap.Droid.Database;

namespace StarMap.Droid
{
  [Activity(Label = "@string/app_name", 
    Theme = "@style/MyTheme", 
    ScreenOrientation = ScreenOrientation.Landscape,
    MainLauncher = true, 
    ConfigurationChanges = ConfigChanges.ScreenSize)]
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
      // new App(new UnityInitializer());
      LoadApplication(new App());
    }
  }
}

