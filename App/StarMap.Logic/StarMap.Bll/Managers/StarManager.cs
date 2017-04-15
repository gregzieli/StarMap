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
      return Call(() =>
      {
        var constellations = _provider.GetConstellations();
        return constellations;
      }, suppress: true, onException: (ex) =>
      {

      });      
    }

    public IEnumerable<Star> GetStars(StarFilter filter)
    {
      return Call(() =>
      {
        var stars = _provider.GetStars(filter);
        return stars;
      });
      
    }

    public StarDetail GetStarDetails(int id)
    {
      return Call(() =>
      {
        var star = _provider.GetStarDetails(id);
        return star;
      });      
    }
  }
}
