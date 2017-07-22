using StarMap.Cll.Filters;
using StarMap.Cll.Models.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarMap.Cll.Abstractions
{
  public interface IStarDataAsyncProvider
  {
    Task<IList<Constellation>> GetConstellationsAsync();

    Task<IEnumerable<Star>> GetStarsAsync(StarFilter filter);

    Task<StarDetail> GetStarDetailsAsync(int id);
  }
}
