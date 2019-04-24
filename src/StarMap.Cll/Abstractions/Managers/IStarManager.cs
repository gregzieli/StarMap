using StarMap.Cll.Filters;
using StarMap.Cll.Models.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarMap.Cll.Abstractions
{
    /// <summary>
    /// Contains methods dedicated to handling star data.
    /// </summary>
    public interface IStarManager
    {
        StarFilter LoadFilter();

        bool CheckFilterChanged(StarFilter filter);

        Task<IList<Constellation>> GetConstellationsAsync();

        Task<IEnumerable<Star>> GetStarsAsync(StarFilter filter);

        Task<StarDetail> GetStarDetailsAsync(int id);
    }
}