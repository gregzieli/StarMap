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
    public StarDatabaseProvider(IDatabaseConnection context) : base(context) { }

    public IEnumerable<Constellation> GetConstellations()
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
          // TODO: Custom ex, Ex strategy!
          throw new Exception("Element missing from the DB!");

        var constellation = entity.ConstellationId != null
          ? context.Find<ConstellationEntity>(entity.ConstellationId)
          : null;

        var retVal = new StarDetail()
        {
          Id = entity.Id,
          HipparcosId = entity.HipparcosId,
          HenryDraperId = entity.HenryDraperId,
          GlieseId = entity.GlieseId,
          Base = entity.Base,
          AbsoluteMagnitude = entity.AbsoluteMagnitude,
          ApparentMagnitude = entity.ApparentMagnitude,
          BayerName = entity.BayerName,
          Color = Colors.MapColor(entity.SpectralType),
          SpectralType = entity.SpectralType,
          Declination = entity.Declination,
          DeclinationRad = entity.DeclinationRad,
          FlamsteedName = entity.FlamsteedName,
          ParsecDistance = entity.ParsecDistance,
          LightYearDistance = Stars.GetLightYears(entity.ParsecDistance),
          Luminosity = entity.Luminosity,
          ProperName = entity.ProperName,
          RightAscension = entity.RightAscension,
          RightAscensionRad = entity.RightAscensionRad,
          X = entity.X,
          Y = entity.Y,
          Z = entity.Z,
          Constellation = Constellations.Map(constellation)
        };
        return retVal;
      });
    }

    public IEnumerable<Star> GetStars(StarFilter filter)
    {
      return Read(context =>
      {
        var query = context.Table<StarEntity>();

        if (!filter.ConstellationsIds.IsNullOrEmpty())
          query = query.Where(x => filter.ConstellationsIds.Contains(x.ConstellationId.GetValueOrDefault()));

        if (filter.MaxDistance.HasValue)
          query = query.Where(x => x.ParsecDistance < filter.MaxDistance.Value);

        if (filter.MaxMagnitude.HasValue)
          query = query.Where(x => x.ApparentMagnitude < filter.MaxMagnitude.Value);

        if (filter.Limit.HasValue)
          query = query.Take(filter.Limit.Value);

        var re = query.AsEnumerable()
          .Select(x => Stars.Map(x));

        return re.ToList();
      });
    }
  }
}
