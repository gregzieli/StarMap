using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;

namespace StarMap.Cll.Abstractions
{
  public interface ILocationManager
  {
    Task<Position> GetGpsPositionAsync();
  }
}
