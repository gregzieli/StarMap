using NUnit.Framework;
using Plugin.Geolocator.Abstractions;
using StarMap.Cll.Models.Geolocation;
using System;
using System.Collections.Generic;

namespace StarMap.LogicTest
{
  [TestFixture]
  public class Location
  {
    [Test]
    public void GetCoordinates()
    {
      var x = new Dictionary<Tuple<double, double>, string>()
      {
        { Tuple.Create(52.261874, 21.000561),    "52°15'43\"N 21°00'02\"E" },
        { Tuple.Create(52.791930, 22.167297),    "52°47'31\"N 22°10'02\"E" },
        { Tuple.Create(68.328253, 17.332272),    "68°19'42\"N 17°19'56\"E" },
        { Tuple.Create(-34.178137, 18.990271),   "34°10'41\"S 18°59'25\"E" },
        { Tuple.Create(-42.588802, 145.854215),  "42°35'20\"S 145°51'15\"E" },
        { Tuple.Create(-84.359950, 179.836874),  "84°21'36\"S 179°50'13\"E" },
        { Tuple.Create(-47.797437, -127.528874), "47°47'51\"S 127°31'44\"W" },
        { Tuple.Create(0.0, -151.340063),        "0°00'00\"N 151°20'24\"W" },
        { Tuple.Create(55.569337, -113.169007),  "55°34'10\"N 113°10'08\"W" },
        { Tuple.Create(83.413072, -33.522526),   "83°24'47\"N 33°31'21\"W" },
        { Tuple.Create(0.000000, 0.000000),      "0°00'00\"N 0°00'00\"E" }
      };

      foreach (var item in x)
      {
        var a = new ExtendedPosition(new Position()
        {
          Latitude = item.Key.Item1,
          Longitude = item.Key.Item2
        });
        Assert.AreEqual(item.Value, $"{a.DmsLatitude.ToString()} {a.DmsLongitude.ToString()}");
      }
    }
  }
}
