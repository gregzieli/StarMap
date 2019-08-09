using StarMap.Cll.Models.Geolocation;
using Xamarin.Essentials;
using Xunit;

namespace StarMap.Cll.Test.Units
{
    public class ExtendedLocationTest
    {
        [Theory]
        [InlineData(52.261874, 21.000561, "52°15'43\"N 21°00'02\"E")]
        [InlineData(52.791930, 22.167297, "52°47'31\"N 22°10'02\"E")]
        [InlineData(68.328253, 17.332272, "68°19'42\"N 17°19'56\"E")]
        [InlineData(-34.178137, 18.990271, "34°10'41\"S 18°59'25\"E")]
        [InlineData(-42.588802, 145.854215, "42°35'20\"S 145°51'15\"E")]
        [InlineData(-84.359950, 179.836874, "84°21'36\"S 179°50'13\"E")]
        [InlineData(-47.797437, -127.528874, "47°47'51\"S 127°31'44\"W")]
        [InlineData(0.0, -151.340063, "0°00'00\"N 151°20'24\"W")]
        [InlineData(55.569337, -113.169007, "55°34'10\"N 113°10'08\"W")]
        [InlineData(83.413072, -33.522526, "83°24'47\"N 33°31'21\"W")]
        [InlineData(0.000000, 0.000000, "0°00'00\"N 0°00'00\"E")]

        public void GetCoordinates(double latitude, double longitude, string expectedString)
        {
            // Assign
            var position = new ExtendedLocation(new Location(latitude, longitude));

            // Act & Assert
            Assert.Equal(expectedString, $"{position.DmsLatitude.ToString()} {position.DmsLongitude.ToString()}");
        }
    }
}
