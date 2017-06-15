using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Models.Geolocation;
using StarMap.ViewModels.Core;
using System;
using System.Threading.Tasks;

namespace StarMap.ViewModels
{
  public class SettingsPageViewModel : Navigator
  {
    ILocationManager _locationManager;
    
    public DelegateCommand UpdateLocationCommand { get; private set; }

    private ExtendedPosition _geoPosition;
    public ExtendedPosition GeoPosition
    {
      get { return _geoPosition; }
      set { SetProperty(ref _geoPosition, value); }
    }

    public SettingsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, 
      ILocationManager locationManager) 
      : base(navigationService, pageDialogService)
    {
      _locationManager = locationManager;
      UpdateLocationCommand = new DelegateCommand(UpdateLocation, CanExecute)
        .ObservesProperty(() => IsBusy);
    }

    // Async void command handler
    private async void UpdateLocation()
    {
      await CallAsync(_locationManager.GetNewGpsPositionAsync,
        position =>
        {
          if (position != null)
            GeoPosition = position;
        });
    }

    protected override async Task Restore(NavigationParameters parameters)
    {
      await CallAsync(async () =>
      {
        GeoPosition = await _locationManager.CheckLocationAsync().ConfigureAwait(false);
        // other settings
      });
    }

    protected override async Task HandleException(Exception ex)
    {
      string message = "Cannot get localization for this device. The position will not update.";
#if DEBUG
      message += Environment.NewLine + "Exception:" + Environment.NewLine + ex.Message;
#endif
      await DisplayAlertAsync("Localization error", message, "OK");
    }
  }
}
