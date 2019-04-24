

namespace StarMap.Cll.Models.Geolocation
{
    public class ExtendedPosition : Position
    {
        public DegreeCoordinate DmsLatitude { get; set; }

        public DegreeCoordinate DmsLongitude { get; set; }

        public ExtendedPosition(Position position) : base(position)
        {
            DmsLatitude = new DegreeCoordinate(position.Latitude, CoordinateType.Latitude);
            DmsLongitude = new DegreeCoordinate(position.Longitude, CoordinateType.Longitude);
        }
    }

    public enum CoordinateType
    {
        Latitude,

        Longitude
    }
}
