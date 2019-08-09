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
