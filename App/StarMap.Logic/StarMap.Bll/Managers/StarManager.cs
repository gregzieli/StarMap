using StarMap.Cll.Abstractions;
using StarMap.Cll.Filters;
using StarMap.Cll.Models;
using System.Collections.Generic;

namespace StarMap.Bll.Managers
{
  public class StarManager : BaseManager, IStarManager
  {
    IStarDataProvider _provider;

    public StarManager(IStarDataProvider provider)
    {
      _provider = provider;
    }

    public IEnumerable<Constellation> GetConstellations()
    {
      var constellations = _provider.GetConstellations();
      return constellations;
    }

    public IEnumerable<Star> GetStars(StarFilter filter)
    {
      var stars = _provider.GetStars(filter);
      return stars;
    }

    public StarDetail GetStarDetails(int id)
    {
      var star = _provider.GetStarDetails(id);
      return star;
    }
  }
}
