using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using StarMap.Bll.Helpers;
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

    public SettingsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ILocationManager locationManager)
      : base(navigationService, pageDialogService)
    {
      _locationManager = locationManager;
    }

    public bool SensorsOn
    {
      get => Settings.SensorsOn;
      // No time to create a manager for that. 
      set { Settings.SensorsOn = value; }
    }

    ExtendedPosition _geoPosition;
    public ExtendedPosition GeoPosition
    {
      get => _geoPosition;
      set { SetProperty(ref _geoPosition, value); }
    }

    DelegateCommand _updateLocationCommand;
    public DelegateCommand UpdateLocationCommand =>
        _updateLocationCommand ?? (_updateLocationCommand = new DelegateCommand(UpdateLocation)
        // If IsBusy didn't RaisePropertyChanged on CanExecute, this code could stay, but with ObservesProperty.
        .ObservesCanExecute(() => CanExecute));
    

    async void UpdateLocation()
    {
      await CallAsync(_locationManager.GetNewGpsPositionAsync,
        position =>
        {
          if (position != null)
            GeoPosition = position;
        });
    }

    protected override async void Restore(NavigationParameters parameters)
    {
      await CallAsync(async () =>
      {
        GeoPosition = await _locationManager.CheckLocationAsync();
        // other settings
      });
    }

    protected override async Task HandleException(Exception ex)
    {
      await DisplayAlertAsync("Localization error", "Cannot get localization for this device. The position will not update.");
    }
  }
}
