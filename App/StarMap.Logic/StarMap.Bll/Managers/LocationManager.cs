using Plugin.Geolocator;
using System;
using System.Threading.Tasks;

namespace StarMap.Bll.Managers
{
  public class LocationManager : BaseManager
  {
    public async Task<double[]> GetGpsLocationAsync()
    {
      return await CallAsync(async () =>
      {
        var locator = CrossGeolocator.Current;
        if (!locator.IsGeolocationEnabled)
          throw new Exception("Location must be enabled.");

        var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

        return new double[] { position.Altitude, position.Latitude, position.Longitude };
       });      
    }
  }
}
