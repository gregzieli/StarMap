using Plugin.Geolocator.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using StarMap.Cll.Abstractions;
using StarMap.ViewModels.Core;
using System;

namespace StarMap.ViewModels
{
  public class SettingsPageViewModel : Interlocutor
  {
    ILocationManager _locationManager;
    
    public DelegateCommand UpdateLocationCommand { get; private set; }

    private Position _geoPosition;
    public Position GeoPosition
    {
      get { return _geoPosition; }
      set { SetProperty(ref _geoPosition, value); }
    }

    public SettingsPageViewModel(INavigationService navigationService, 
      IPageDialogService dialogService, 
      ILocationManager locationManager) 
      : base(navigationService, dialogService)
    {
      _locationManager = locationManager;
      UpdateLocationCommand = new DelegateCommand(UpdateLocation, CanExecute).ObservesProperty(() => IsBusy);
    }

    // From what I read async void (fire and forget) is bad practice, however,
    // it's the only way to be used as a delegate (for a Command or EventHandler).
    private async void UpdateLocation()
    {
      await CallAsync(async () =>
      {
        GeoPosition = await _locationManager.GetGpsPositionAsync();
      }, onException: async (ex) =>
      {
        string message = "Cannot get localization for this device. The position will not update.";
#if DEBUG
        message += Environment.NewLine + "Exception:" + Environment.NewLine + ex.Message;
#endif
        await DisplayAlertAsync("Localization error", message, "OK");
      });
    }

    public override void OnNavigatingTo(NavigationParameters parameters)
    {
      if (parameters.ContainsKey("init"))
        UpdateLocation();
    }
  }
}
