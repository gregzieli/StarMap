using StarMap.Cll.Models;
using DbEntityConstellation = StarMap.Dal.Database.Contracts.Constellation;

namespace StarMap.Dal.Mappers
{
  public static class Constellations
  {
    public static Constellation Map(DbEntityConstellation source)
    {
      if (source == null)
        return null;

      return new Constellation()
      {
        Id = source.Id,
        Name = source.Name,
        Abbreviation = source.Abbreviation
      };
    }
  }
}
