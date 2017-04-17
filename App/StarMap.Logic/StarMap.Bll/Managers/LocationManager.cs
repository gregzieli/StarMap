using Plugin.Geolocator;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Models.Geolocation;
using System;
using System.Threading.Tasks;

namespace StarMap.Bll.Managers
{
  public class LocationManager : BaseManager, ILocationManager
  {    
    // TODO: make the Getlocator method from an interface, so that the 'CrossGeolocactor' itself can be mocked.
    public LocationManager()
    {

    }

    public async Task<ExtendedPosition> GetGpsPositionAsync()
    {
      var locator = CrossGeolocator.Current;
      if (!locator.IsGeolocationEnabled)
        // TODO: custom ex
        throw new Exception("Location must be enabled.");

      // In meters
      locator.DesiredAccuracy = 50;

      var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
      return new ExtendedPosition(position);
    }
  }
}
