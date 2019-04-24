using StarMap.Cll.Abstractions;
using StarMap.Cll.Abstractions.Services;
using StarMap.Cll.Filters;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarMap.Bll.Managers
{
    public class StarManager : BaseManager, IStarManager
    {
        private readonly IStarDataAsyncProvider _provider;
        private readonly IAstronomer _astronomer;
        private readonly ISettingsManager _settingsManager;

        public StarManager(IStarDataAsyncProvider provider, IAstronomer astronomer, ISettingsManager settingsManager)
        {
            _provider = provider;
            _astronomer = astronomer;
            _settingsManager = settingsManager;
        }

        public async Task<IList<Constellation>> GetConstellationsAsync()
        {
            var constellations = await _provider.GetConstellationsAsync();

            if (constellations.IsNullOrEmpty())
                throw new Exception("Constellations missing from the database");

            return constellations;
        }

        public async Task<IEnumerable<Star>> GetStarsAsync(StarFilter filter)
        {
            _settingsManager.Filter = Serialize(filter);

            var stars = await _provider.GetStarsAsync(filter);

            if (stars.IsNullOrEmpty())
                throw new Exception("Stars missing from the database");

            return stars;
        }

        public async Task<StarDetail> GetStarDetailsAsync(int id)
        {
            var star = await _provider.GetStarDetailsAsync(id);

            PrepareStar(star);

            return star;
        }

        public bool CheckFilterChanged(StarFilter filter)
        {
            var previous = LoadFilter();

            return !filter.Equals(previous);
        }

        public StarFilter LoadFilter()
        {
            var filter = _settingsManager.Filter;
            var defReturn = new StarFilter();

            return filter.IsNullOrEmpty() ? defReturn : Deserialize(filter, defReturn);
        }

        private void PrepareStar(StarDetail star)
        {
            star.Color = _astronomer.GetColor(star.SpectralType);
            if (star.ColorIndex.HasValue)
                star.TemperatureKelvin = _astronomer.GetTemperature(star.ColorIndex.Value);
        }
    }
}
