using Prism.AppModel;
using Prism.Navigation;
using Prism.Services;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Abstractions.Urho;
using StarMap.Cll.Constants;
using StarMap.Urhosharp;
using System.Threading;
using System.Threading.Tasks;
using Urho;
using Urho.Forms;
using XF = Xamarin.Forms;

namespace StarMap.ViewModels.Core
{
    public abstract class StarGazer<TUhroApp, TUrhoException> : Navigator, IUrhoHandler, IApplicationLifecycleAware, IDestructible
      where TUhroApp : UrhoBase
      where TUrhoException : System.Exception
    {
        protected IStarManager StarManager;
        public TUhroApp UrhoApplication { get; set; }


        public StarGazer(INavigationService navigationService, IPageDialogService pageDialogService, IStarManager starManager)
          : base(navigationService, pageDialogService)
        {
            StarManager = starManager;
        }

        private async void UrhoUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            // Some stuff get caught here, some don't. For example an error during init doesn't get caught here, that's why
            // it has its own trycatch, and a MessagingCenter event handling.
            if (!UrhoApplication.IsExiting)
            {
                await UrhoApplication.Exit().ConfigureAwait(false);
                await HandleException(e.Exception).ConfigureAwait(false);
            }
        }

        protected override void Restore(INavigationParameters parameters)
        {
            XF.MessagingCenter.Subscribe<TUrhoException>(this, MessageKeys.UrhoError, async ex => await HandleException(ex));
        }

        protected override void CleanUp()
        {
            XF.MessagingCenter.Unsubscribe<TUrhoException>(this, MessageKeys.UrhoError);
        }

        public async Task GenerateUrho(UrhoSurface surface)
        {
            await CallAsync(async () =>
            {
                // As it happens, MainPanel's Urho doesn't get disposed before 
                // starting this one. Believe me, I tried many other ways.
                await Task.Delay(100);

                // Moving this piece of code to here from the View doesn't change much, it's really just for consistency;
                // I still am unable to catch any exception that occurs upon creating the UrhoApplication.
                // That is why it is needed to be handled separately, as another layer.
                // An error on OnUrhoGenerated can be caught here.
                var options = new ApplicationOptions(assetsFolder: "Data");

                // Unbelievable. After almost a year, navigating back and forth still doesn't work properly.
                // This appears to solve the issue.
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

                UrhoApplication = await surface.Show<TUhroApp>(options);
            }, onDone: () => OnUrhoGenerated());
        }

        public virtual void OnResume() => UrhoSurface.OnResume();

        public virtual void OnSleep() => UrhoSurface.OnPause();

        public abstract void OnUrhoGenerated();

        public void Destroy()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            Thread.Sleep(100);
            UrhoSurface.OnDestroy();
        }
    }
}