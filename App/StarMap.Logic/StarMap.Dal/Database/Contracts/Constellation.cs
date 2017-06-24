using SQLite;
using StarMap.Core.Abstractions;
using System.Collections.Generic;

namespace StarMap.Dal.Database.Contracts
{
  public class Constellation : IUnique
  {
    [PrimaryKey, AutoIncrement, Column("_id")]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Abbreviation { get; set; }

    public List<Star> Stars { get; set; }

    public Constellation() { }
  }
}
