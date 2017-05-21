using Prism.Unity;
using StarMap.ViewModels;
using StarMap.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace StarMap
{  
  public partial class App : PrismApplication
  {
    public App(IPlatformInitializer initializer = null) : base(initializer) { }

    protected override void OnInitialized()
    {
      TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
      try
      {
        InitializeComponent();
        NavigationService.NavigateAsync("MasterDetail/StartPage");
      }
      catch (Exception e)
      {
        ShowCrashPage(e);
      }
    }
    
    private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
      if (e.Observed) return;

      //prevents the app domain from being torn down 
      // https://oceanware.wordpress.com/2016/12/01/ocean-validation-for-xamarin-forms/
      e.SetObserved();
      ShowCrashPage(e.Exception.Flatten().GetBaseException());
    }
    
    // TODO: Or just a popup and close app
    // cannot close app programaticaly?
    void ShowCrashPage(Exception e = null)
    {
      // setting of the MainPage should clear the stack.
      //Device.BeginInvokeOnMainThread(() => MainPage = new CrashPage(e?.Message));
    }

    protected override void RegisterTypes()
    {
      // Register other (logical) types in a separate method
      UnityBootstrapper.RegisterTypes(Container);

      Container.RegisterTypeForNavigation<NavigationPage>();
      Container.RegisterTypeForNavigation<StartPage, StartPageViewModel>();
      Container.RegisterTypeForNavigation<MasterDetail, MasterDetailViewModel>();
      Container.RegisterTypeForNavigation<StarDetailPage, StarDetailPageViewModel>();
      Container.RegisterTypeForNavigation<SettingsPage>();
      Container.RegisterTypeForNavigation<AboutPage>();
      Container.RegisterTypeForNavigation<MainPage>();
    }
    
    protected override void OnSleep()
    {
      base.OnSleep();

      //var sensor = new SensorManager();
      //sensor.Stop();
    }
  }
}
