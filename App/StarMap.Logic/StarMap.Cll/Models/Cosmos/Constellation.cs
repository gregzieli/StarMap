using StarMap.Cll.Models.Core.Bindable;
using System.Collections.Generic;

namespace StarMap.Cll.Models.Cosmos
{
  public class Constellation : MultiSelectable
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Abbreviation { get; set; }

    public List<Star> Stars { get; set; }
  }
}
