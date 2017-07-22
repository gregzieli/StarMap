using StarMap.Bll.Helpers;
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
    IStarDataAsyncProvider _provider;
    // There really is no reason to not store the logic in Astronomer just here, in another method.
    // But i like unity.
    IAstronomer _astronomer;

    public StarManager(IStarDataAsyncProvider provider, IAstronomer astronomer)
    {
      _provider = provider;
      _astronomer = astronomer;
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
      Settings.Filter = Serialize(filter);

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

    public StarFilter LoadFilter()
    {
      string filter = Settings.Filter;
      var defReturn = new StarFilter();

      if (filter.IsNullOrEmpty())
        return defReturn;

      return Deserialize(filter, defReturn);
    }

    void PrepareStar(StarDetail star)
    {
      star.Color = _astronomer.GetColor(star.SpectralType);
      if (star.ColorIndex.HasValue)
        star.TemperatureKelvin = _astronomer.GetTemperature(star.ColorIndex.Value);
    }
  }
}
