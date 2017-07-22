﻿using StarMap.Cll.Filters;
using StarMap.Cll.Models.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarMap.Cll.Abstractions
{
  public interface IStarManager
  {
    IList<Constellation> GetConstellations();

    IEnumerable<Star> GetStars(StarFilter filter);

    StarDetail GetStarDetails(int id);

    StarFilter LoadFilter();

    Task<IList<Constellation>> GetConstellationsAsync();

    Task<IEnumerable<Star>> GetStarsAsync(StarFilter filter);

    Task<StarDetail> GetStarDetailsAsync(int id);
  }
}
