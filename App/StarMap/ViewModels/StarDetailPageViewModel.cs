using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Constants;
using StarMap.Cll.Events;
using StarMap.Cll.Models.Cosmos;
using StarMap.Urho;
using StarMap.ViewModels.Core;
using System.Threading.Tasks;
using Urho;
using Urho.Forms;
using System;
using StarMap.Cll.Exceptions;

namespace StarMap.ViewModels
{
  public class StarDetailPageViewModel : Navigator
  {
    IStarManager _starManager;

    private StarDetail _star;
    public StarDetail Star
    {
      get { return _star; }
      set { SetProperty(ref _star, value); }
    }
    
    public StarDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IEventAggregator eventAggregator, IStarManager starManager) 
      : base(navigationService, pageDialogService)
    {
      _starManager = starManager;
      eventAggregator.GetEvent<UrhoErrorEvent<StarDetailUrhoException>>().Subscribe(async(ex) => await HandleException(ex), ThreadOption.BackgroundThread);
    }

    public override void OnNavigatedTo(NavigationParameters parameters)
    {
      base.OnNavigatedTo(parameters);
      Star = _starManager.GetStarDetails((int)parameters[Navigation.Keys.StarId]);
    }

    public async Task GenerateUrho(UrhoSurface surface)
    {
      // Moving this piece of code to here from the View doesn't change much, it's really just for consistency;
      // I still am unable to catch any exception that occurs upon creating the UrhoApplication.
      // That is why it is needed to be handled separately, as another layer.
      // An error on SetStar can be caught here.
      await CallAsync(async () =>
      {
        var options = new ApplicationOptions(assetsFolder: "Data")
        {
          //Orientation = Urho.ApplicationOptions.OrientationType.LandscapeAndPortrait,
          // iOS only - which is a shame, because now I have to ensure the view height < width
          // from https://github.com/xamarin/urho/blob/master/Urho3D/Urho3D_Android/Sdl/SDLSurface.java
          // if (requestedOrientation == ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE) 
          //  if (mWidth < mHeight) skip = true;
          // and with skip=true nothing happens, with log Log.v("SDL", "Skip .. Surface is not ready.");        
        };

        var urho = await surface.Show<SingleStar>(options).ConfigureAwait(continueOnCapturedContext: false);
        urho.Star = Star;
      });      
    }
  }
}
