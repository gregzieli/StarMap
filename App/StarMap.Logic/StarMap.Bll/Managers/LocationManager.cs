using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using StarMap.Cll.Abstractions;
using System;
using System.Threading.Tasks;

namespace StarMap.Bll.Managers
{
  public class LocationManager : BaseManager, ILocationManager
  {
    //public async Task<double[]> GetGpsLocationAsync()
    //{
    //  return await CallAsync(async () =>
    //  {
    //    var locator = CrossGeolocator.Current;
    //    if (!locator.IsGeolocationEnabled)
    //      throw new Exception("Location must be enabled.");

    //    var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

    //    return new double[] { position.Altitude, position.Latitude, position.Longitude };
    //   });      
    //}
    
    // TODO: make the Getlocator method from an interface, so that the 'CrossGeolocactor' itself can be mocked.
    public LocationManager()
    {

    }

    public async Task<Position> GetGpsPositionAsync()
    {
      var locator = CrossGeolocator.Current;
      if (!locator.IsGeolocationEnabled)
        // TODO: custom ex
        throw new Exception("Location must be enabled.");

      // In meters
      locator.DesiredAccuracy = 50;

      return await locator.GetPositionAsync(timeoutMilliseconds: 10000);
    }
  }
}
