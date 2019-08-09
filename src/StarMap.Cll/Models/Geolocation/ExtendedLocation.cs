using Xamarin.Essentials;

namespace StarMap.Cll.Models.Geolocation
{
    public class ExtendedLocation : Location
    {
        public DegreeCoordinate DmsLatitude { get; set; }

        public DegreeCoordinate DmsLongitude { get; set; }

        public ExtendedLocation(Location location) : base(location)
        {
            DmsLatitude = new DegreeCoordinate(location.Latitude, CoordinateType.Latitude);
            DmsLongitude = new DegreeCoordinate(location.Longitude, CoordinateType.Longitude);
        }
    }
}
