using StarMap.Cll.Abstractions;
using StarMap.Cll.Abstractions.Services;
using StarMap.Cll.Filters;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Extensions;
using System;
using System.Collections.Generic;

namespace StarMap.Bll.Managers
{
  public class StarManager : BaseManager, IStarManager
  {
    IStarDataProvider _provider;
    IStarPainter _painter;

    public StarManager(IStarDataProvider provider, IStarPainter painter)
    {
      _provider = provider;
      _painter = painter;
    }

    public IList<Constellation> GetConstellations()
    {
      var constellations = _provider.GetConstellations();

      if (constellations.IsNullOrEmpty())
        throw new Exception("Constellations missing from the database");

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
      star.Color = _painter.GetColor(star);
      return star;
    }
  }
}
