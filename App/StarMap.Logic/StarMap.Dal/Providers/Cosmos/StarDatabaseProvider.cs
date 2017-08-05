using StarMap.Cll.Abstractions;
using StarMap.Cll.Filters;
using StarMap.Cll.Models;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Extensions;
using StarMap.Dal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using ConstellationEntity = StarMap.Dal.Database.Contracts.Constellation;
using StarEntity = StarMap.Dal.Database.Contracts.Star;

namespace StarMap.Dal.Providers
{
  public class StarDatabaseProvider : DatabaseProvider, IStarDataProvider
  {
    // Inject the context (supports mock)
    public StarDatabaseProvider(IRepository repository) : base(repository) { }

    public IList<Constellation> GetConstellations()
    {
      return Read(context =>
      {
        var re = context.Table<ConstellationEntity>()
          .AsEnumerable()
          .Select(x => Constellations.Map(x));
        // Since the connection is closed after this method ends, ToList() is required.
        return re.ToList();
      });
    }
    
    public StarDetail GetStarDetails(int id)
    {
      return Read(context =>
      {
        var entity = context.Find<StarEntity>(id);

        if (entity == null)
          throw new Exception("Element missing from the DB!");

        entity.Constellation = context.Find<ConstellationEntity>(entity.ConstellationId);

        var retVal = Stars.MapDetail(entity);
        return retVal;
      });
    }

    public IEnumerable<Star> GetStars(StarFilter filter)
    {
      return Read(context =>
      {
        var query = context.Table<StarEntity>()
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
        
        var re = query.AsEnumerable()
          .Select(x => Stars.Map(x));

        return re.ToList();
      });
    }
  }
}
