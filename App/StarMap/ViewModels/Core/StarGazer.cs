using Prism.Navigation;
using Prism.Services;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Abstractions.Urho;
using System;
using System.Threading.Tasks;
using Urho;
using Urho.Forms;
using XF = Xamarin.Forms;

namespace StarMap.ViewModels.Core
{
  // Not sure about this name now. Let's just assume, that this is just to take some load off the MainPageVM, not to store all the code in one place; 
  // The ethods here are more 'GENERAL', but I don't think any other VM would inherit this class.
  public abstract class StarGazer<TUhroApp, TUrhoException> : Navigator, IUrhoHandler
    where TUhroApp : Application
    where TUrhoException : Exception
  {
    protected IStarManager _starManager;
    public TUhroApp UrhoApplication { get; set; }


    public StarGazer(INavigationService navigationService, IPageDialogService pageDialogService, IStarManager starManager)
      : base(navigationService, pageDialogService)
    {
      _starManager = starManager;
      // So if I use this, the Catch in CallAsync doesn't get used. 
      // And that implementation works smoothly, whereas this causes more and more problems.
      // Leave it for future reference.
      //Application.UnhandledException += UrhoUnhandledException;
    }

    private void UrhoUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      e.Handled = true;
      // Some stuff get caught here, some don't. For example an error during init doesn't get caught here, that's why
      // it has its own trycatch, and a MessagingCenter event handling.
      //if (!UrhoApplication.IsExiting)
      //{
      //  await UrhoApplication.Exit().ConfigureAwait(false);
      //  await HandleException(e.Exception).ConfigureAwait(false);
      //}        
    }

    protected override void Restore(NavigationParameters parameters)
    {
      XF.MessagingCenter.Subscribe<TUrhoException>(this, string.Empty, async (ex) => await HandleException(ex));
    }

    protected override void CleanUp()
    {
      XF.MessagingCenter.Unsubscribe<TUrhoException>(this, string.Empty);
    }

    public async Task GenerateUrho(UrhoSurface surface)
    {      
      await CallAsync(async () =>
      {
        // Moving this piece of code to here from the View doesn't change much, it's really just for consistency;
        // I still am unable to catch any exception that occurs upon creating the UrhoApplication.
        // That is why it is needed to be handled separately, as another layer.
        // An error on OnUrhoGenerated can be caught here.
        var options = new ApplicationOptions(assetsFolder: "Data")
        {
          //Orientation = Urho.ApplicationOptions.OrientationType.LandscapeAndPortrait,
          // iOS only - which is a shame, because now I have to ensure the view height < width
          // from https://github.com/xamarin/urho/blob/master/Urho3D/Urho3D_Android/Sdl/SDLSurface.java
          // if (requestedOrientation == ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE) 
          //  if (mWidth < mHeight) skip = true;
          // and with skip=true nothing happens, with log Log.v("SDL", "Skip .. Surface is not ready.");        
        };

        if (this is StarDetailPageViewModel)
        {
          XF.Device.BeginInvokeOnMainThread(async () =>
          {
            UrhoApplication = await surface.Show<TUhroApp>(options);
            OnUrhoGenerated();
          });
        }
        else
        {
          UrhoApplication = await surface.Show<TUhroApp>(options);
          OnUrhoGenerated();
        }
      });
    }

    public abstract void OnUrhoGenerated();
  }
}