using StarMap.Cll.Filters;
using StarMap.Cll.Models;
using System.Collections.Generic;

namespace StarMap.Cll.Abstractions
{
  public interface IStarManager
  {
    IEnumerable<Constellation> GetConstellations();

    IEnumerable<Star> GetStars(StarFilter filter);

    StarDetail GetStarDetails(int id);
  }
}
