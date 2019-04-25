using StarMap.Cll.Abstractions;
using StarMap.Cll.Abstractions.Managers;
using StarMap.Cll.Models.Geolocation;
using StarMap.Core.Extensions;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace StarMap.Bll.Managers
{
    public class LocationManager : ILocationManager
    {
        private readonly ISettingsManager _settingsManager;
        private readonly ISerializationManager _serializationManager;

        public LocationManager(ISettingsManager settingsManager, ISerializationManager serializationManager)
        {
            _settingsManager = settingsManager;
            _serializationManager = serializationManager;
        }

        // TODO: in the Android project:
        // https://docs.microsoft.com/en-us/xamarin/essentials/geolocation?context=xamarin%2Fxamarin-forms&tabs=android#get-started

        // TODO: Also this: https://docs.microsoft.com/en-us/xamarin/essentials/get-started?tabs=windows%2Candroid
        public async Task<ExtendedLocation> GetNewGpsPositionAsync()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.GetLocationAsync(request).ConfigureAwait(false);

            _settingsManager.Geolocation = _serializationManager.Serialize(location);

            return new ExtendedLocation(location);
        }

        public async Task<ExtendedLocation> CheckLocationAsync()
        {
            var p = _settingsManager.Geolocation;
            if (p.IsNullOrEmpty())
                return await GetNewGpsPositionAsync();

            var position = _serializationManager.Deserialize<Location>(p);

            return new ExtendedLocation(position);
        }
    }
}
