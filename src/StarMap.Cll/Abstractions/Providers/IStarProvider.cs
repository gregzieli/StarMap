using StarMap.Cll.Filters;
using StarMap.Cll.Models.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarMap.Cll.Abstractions
{
    /// <summary>
    /// Contains methods tasked to retrieve related Universe informations asynchronously.
    /// </summary>
    public interface IStarProvider
    {
        Task<IList<Constellation>> GetConstellationsAsync();

        Task<IEnumerable<Star>> GetStarsAsync(StarFilter filter);

        Task<StarDetail> GetStarDetailsAsync(int id);
    }
}
