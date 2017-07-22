using StarMap.Cll.Abstractions;
using StarMap.Cll.Filters;
using StarMap.Cll.Models;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Extensions;
using StarMap.Dal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstellationEntity = StarMap.Dal.Database.Contracts.Constellation;
using StarEntity = StarMap.Dal.Database.Contracts.Star;

namespace StarMap.Dal.Providers
{
  public class StarDatabaseAsyncProvider : DatabaseAsyncProvider, IStarDataAsyncProvider
  {
    // Inject the context (supports mock)
    public StarDatabaseAsyncProvider(IDatabaseAsyncConnection context) : base(context) { }

    public async Task<IList<Constellation>> GetConstellationsAsync()
    {
      var constellations = await Context.Table<ConstellationEntity>()
        .ToListAsync().ConfigureAwait(false);
      
      return constellations
        .Select(x => Constellations.Map(x))
        .ToList();
    }
    
    public async Task<StarDetail> GetStarDetailsAsync(int id)
    {
      var entity = await Context.FindAsync<StarEntity>(id);

      if (entity == null)
        throw new Exception("Element missing from the DB!");

      entity.Constellation = await Context.FindAsync<ConstellationEntity>(entity.ConstellationId).ConfigureAwait(false);
      
      return Stars.MapDetail(entity);
    }

    public async Task<IEnumerable<Star>> GetStarsAsync(StarFilter filter)
    {
      var query = Context.Table<StarEntity>()
          .Where(x =>
            x.ParsecDistance <= filter.DistanceTo
            && x.ApparentMagnitude <= filter.MagnitudeTo);

      string search = filter.DesignationQuery;
      if (!search.IsNullOrWhiteSpace())
        query = query.Where(x => x.ProperName.Contains(search)
         || x.BayerName.Contains(search)
         || x.FlamsteedName.Contains(search)
         || search.Contains(x.ProperName)
         || search.Contains(x.BayerName)
         || search.Contains(x.FlamsteedName));

      if (!filter.ConstellationsIds.IsNullOrEmpty())
        query = query.Where(x => filter.ConstellationsIds.Contains(x.ConstellationId.GetValueOrDefault()));

      if (filter.Limit.HasValue)
        query = query.Take(filter.Limit.Value);

      var re = (await query.ToListAsync().ConfigureAwait(false))
        .Select(x => Stars.Map(x));

      return re.ToList();
    }
  }
}
