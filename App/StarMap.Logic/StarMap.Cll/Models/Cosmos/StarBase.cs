using System;
using StarMap.Cll.Models.Core;
using StarMap.Core.Abstractions;
using StarMap.Core.Extensions;

namespace StarMap.Cll.Models.Cosmos
{
  public abstract class StarBase : IUnique, IReferencable
  {
    public int Id { get; set; }

    public int? HipparcosId { get; set; }

    public string Name { get; set; }

    public string Bayer { get; set; }

    public string Flamsteed { get; set; }

    public double ParsecDistance { get; set; }

    public double LightYearDistance => ParsecDistance * 3.262;

    public string Designation => Name ?? (!Flamsteed.IsNullOrEmpty() ? $"{Flamsteed} {Bayer}" : Bayer);

    // Keep it in case I dont want it here, but in manager or some utils class in CLL.
    //public static string GetStarDesignation(string proper, string bayer, string flamsteed)
    //  => proper ?? (!flamsteed.IsNullOrEmpty() ? $"{flamsteed} {bayer}" : bayer);
    //public static double GetLightYears(double parsecs)
    //  => parsecs* 3.262;

    //string GetDesignation()
    //{
    //  return Name ?? $"hip: {HipparcosId}, {Flamsteed}{Bayer}"
    //}
  }
}
