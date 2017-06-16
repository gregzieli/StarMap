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
    }

    protected override async Task Restore(NavigationParameters parameters)
    {
      await Call(() => XF.MessagingCenter.Subscribe<TUrhoException>(this, string.Empty, async (ex) => await HandleException(ex)));
    }

    protected override async Task CleanUp()
    {
      await Call(() => XF.MessagingCenter.Unsubscribe<TUrhoException>(this, string.Empty));
    }

    public async Task GenerateUrho(UrhoSurface surface)
    {
      // Moving this piece of code to here from the View doesn't change much, it's really just for consistency;
      // I still am unable to catch any exception that occurs upon creating the UrhoApplication.
      // That is why it is needed to be handled separately, as another layer.
      // An error on SetStar can be caught here.
      var options = new ApplicationOptions(assetsFolder: "Data")
      {
        //Orientation = Urho.ApplicationOptions.OrientationType.LandscapeAndPortrait,
        // iOS only - which is a shame, because now I have to ensure the view height < width
        // from https://github.com/xamarin/urho/blob/master/Urho3D/Urho3D_Android/Sdl/SDLSurface.java
        // if (requestedOrientation == ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE) 
        //  if (mWidth < mHeight) skip = true;
        // and with skip=true nothing happens, with log Log.v("SDL", "Skip .. Surface is not ready.");        
      };

      UrhoApplication = await surface.Show<TUhroApp>(options).ConfigureAwait(continueOnCapturedContext: false);

      await CallAsync(OnUrhoGenerated);
    }

    public abstract Task OnUrhoGenerated();
  }
}
