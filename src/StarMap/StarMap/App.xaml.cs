using Prism;
using Prism.Ioc;
using StarMap.Bll.Helpers;
using StarMap.Bll.Managers;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Abstractions.Managers;
using StarMap.Cll.Abstractions.Providers;
using StarMap.Cll.Abstractions.Services;
using StarMap.Dal.Providers;
using StarMap.ViewModels;
using StarMap.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StarMap
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            try
            {
                InitializeComponent();
                await NavigationService.NavigateAsync("MasterDetail/StartPage");
            }
            catch (Exception e)
            {
                ShowCrashPage(e);
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IConnectionProvider, ConnectionProvider>();
            containerRegistry.Register<ISettingsManager, SettingsManager>();
            containerRegistry.Register<ISerializationManager, SerializationManager>();
            containerRegistry.Register<ILocationManager, LocationManager>();
            containerRegistry.Register<IAstronomer, Astronomer>();
            containerRegistry.Register<IStarManager, StarManager>();
            containerRegistry.Register<IStarProvider, StarProvider>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<StartPage, StartPageViewModel>();
            containerRegistry.RegisterForNavigation<MasterDetail, MasterDetailViewModel>();
            containerRegistry.RegisterForNavigation<StarDetailPage, StarDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<AboutPage, AboutPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<WebViewPage, WebViewPageViewModel>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            if (e.Observed)
                return;

            //prevents the app domain from being torn down 
            // https://oceanware.wordpress.com/2016/12/01/ocean-validation-for-xamarin-forms/
            e.SetObserved();
            ShowCrashPage(e.Exception.Flatten().GetBaseException());
        }

        // TODO: Or just a popup and close app
        // cannot close app programaticaly?
        private void ShowCrashPage(Exception e = null)
        {
            // setting of the MainPage should clear the stack.
            //Device.BeginInvokeOnMainThread(() => MainPage = new CrashPage(e?.Message));
        }
    }
}
