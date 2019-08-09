using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using StarMap.Bll.Managers;
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
        readonly SettingsManager _settingsManager;

        public SettingsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ILocationManager locationManager, SettingsManager settingsManager)
      : base(navigationService, pageDialogService)
        {
            _locationManager = locationManager;
            _settingsManager = settingsManager;
        }

        public bool SensorsOn
        {
            get => _settingsManager.SensorsOn;
            // No time to create a manager for that. 
            set { _settingsManager.SensorsOn = value; }
        }

        ExtendedLocation _geoPosition;
        public ExtendedLocation GeoPosition
        {
            get => _geoPosition;
            set => SetProperty(ref _geoPosition, value);
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

        protected override async void Restore(INavigationParameters parameters)
        {
            await CallAsync(async () =>
            {
                GeoPosition = await _locationManager.CheckLocationAsync();
            });
        }

        protected override Task HandleException(Exception ex)
            => DisplayAlertAsync(ex.Message, "Cannot get localtion for this device. The position will not update.");
    }
}
