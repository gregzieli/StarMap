using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using StarMap.Bll.Helpers;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Models.Geolocation;
using StarMap.Core.Extensions;
using System;
using System.Threading.Tasks;

namespace StarMap.Bll.Managers
{
  public class LocationManager : BaseManager, ILocationManager
  {
    public async Task<ExtendedPosition> GetNewGpsPositionAsync()
    {
      var locator = CrossGeolocator.Current;
      if (!locator.IsGeolocationEnabled)
        throw new Exception("Location must be enabled.");

      // In meters
      locator.DesiredAccuracy = 50;

      var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
        //.ConfigureAwait(continueOnCapturedContext: false);

      StoreGpsPosition(position);

      return new ExtendedPosition(position);
    }

    public async Task<ExtendedPosition> CheckLocationAsync()
    {
      var p = Settings.Geolocation;
      if (p.IsNullOrEmpty())
        return await GetNewGpsPositionAsync();

      var position = Deserialize<Position>(p);

      return new ExtendedPosition(position);
    }

    public void StoreGpsPosition(Position position)
    {
      Settings.Geolocation = Serialize(position);
    }
  }
}
