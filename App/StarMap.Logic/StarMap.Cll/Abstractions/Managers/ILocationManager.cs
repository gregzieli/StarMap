using StarMap.Cll.Models.Geolocation;
using System.Threading.Tasks;

namespace StarMap.Cll.Abstractions
{
  public interface ILocationManager
  {
    Task<ExtendedPosition> GetGpsPositionAsync();
  }
}
