using StarMap.Cll.Abstractions;
using StarMap.Cll.Filters;
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
        public StarDatabaseAsyncProvider(IRepository repository) : base(repository) { }

        public async Task<IList<Constellation>> GetConstellationsAsync()
        {
            var constellations = await Read(x => x.Table<ConstellationEntity>().ToListAsync());

            return constellations
              .Select(x => Constellations.Map(x))
              .ToList();
        }

        public async Task<StarDetail> GetStarDetailsAsync(int id)
        {
            var star = await Read(async x =>
            {
                var entity = await x.FindAsync<StarEntity>(id);

                if (entity == null)
                    throw new Exception("Element missing from the DB!");

                entity.Constellation = await x.FindAsync<ConstellationEntity>(entity.ConstellationId);

                return entity;
            });

            return Stars.MapDetail(star);
        }

        public async Task<IEnumerable<Star>> GetStarsAsync(StarFilter filter)
        {
            var stars = await Read(async context =>
            {
                var query = context.Table<StarEntity>()
                    .Where(x => x.ParsecDistance <= filter.DistanceTo) // TODO: make it a Query Object Pattern
                    .Where(x => x.ApparentMagnitude <= filter.MagnitudeTo);

                var search = filter.DesignationQuery;
                if (!search.IsNullOrWhiteSpace())
                {
                    query = query.Where(x => x.ProperName.Contains(search)
                     || x.BayerName.Contains(search)
                     || x.FlamsteedName.Contains(search)
                     || search.Contains(x.ProperName)
                     || search.Contains(x.BayerName)
                     || search.Contains(x.FlamsteedName));
                }
                var list = await query.ToListAsync();

                // Ensure the Sun is always there
                if (!list.Any(x => x.Id == 0))
                {
                    list.Add(await context.FindAsync<StarEntity>(0));
                }

                return list;
            });

            return stars.Select(Stars.Map);
        }
    }
}
