using StarMap.Cll.Models.Geolocation;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace StarMap.Cll.Abstractions
{
    public interface ILocationManager
    {
        /// <summary>
        /// Checks the device geolocation.
        /// </summary>
        /// <returns>The device location.</returns>
        /// <exception cref="FeatureNotSupportedException"></exception>
        /// <exception cref="FeatureNotEnabledException"></exception>
        /// <exception cref="PermissionException"></exception>
        Task<ExtendedLocation> GetNewGpsPositionAsync();

        Task<ExtendedLocation> CheckLocationAsync();
    }
}
