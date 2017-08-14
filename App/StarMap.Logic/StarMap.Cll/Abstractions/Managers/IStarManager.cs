using StarMap.Cll.Filters;
using StarMap.Cll.Models.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarMap.Cll.Abstractions
{
  public interface IStarManager
  {
    StarFilter LoadFilter();

    bool CheckFilterChanged(StarFilter filter);

    Task<IList<Constellation>> GetConstellationsAsync();

    Task<IEnumerable<Star>> GetStarsAsync(StarFilter filter);

    Task<StarDetail> GetStarDetailsAsync(int id);
  }
}
