using StarMap.Cll.Models.Cosmos;
using DbEntityStar = StarMap.Dal.Database.Contracts.Star;

namespace StarMap.Dal.Mappers
{
    public static class Stars
    {
        /// <summary>
        /// Maps an object from the Star table to the application Star object.
        /// </summary>
        public static Star Map(DbEntityStar source)
        {
            if (source == null)
                return null;

            return new Star()
            {
                Id = source.Id,
                HipparcosId = source.HipparcosId,
                AbsoluteMagnitude = source.AbsoluteMagnitude,
                Name = source.ProperName,
                Bayer = source.BayerName,
                Flamsteed = source.FlamsteedName,
                ConstellationId = source.ConstellationId,
                ParsecDistance = source.ParsecDistance,
                X = source.X,
                Y = source.Y,
                Z = source.Z
            };
        }

        /// <summary>
        /// Maps an object from the Star table to the application StarDetail object.
        /// </summary>
        public static StarDetail MapDetail(DbEntityStar source)
        {
            if (source == null)
                return null;

            return new StarDetail()
            {
                Id = source.Id,
                HipparcosId = source.HipparcosId,
                HenryDraperId = source.HenryDraperId,
                GlieseId = source.GlieseId,
                AbsoluteMagnitude = source.AbsoluteMagnitude,
                ApparentMagnitude = source.ApparentMagnitude,
                Bayer = source.BayerName,
                SpectralType = source.SpectralType,
                Flamsteed = source.FlamsteedName,
                ParsecDistance = source.ParsecDistance,
                Luminosity = source.Luminosity,
                ColorIndex = source.ColorIndex,
                Name = source.ProperName,
                Constellation = Constellations.Map(source.Constellation)
            };
        }
    }
}
