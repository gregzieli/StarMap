using StarMap.Cll.Models.Core;

namespace StarMap.Cll.Models.Cosmos
{
  public class Planet : IReferencable
  {
    public string Designation { get; private set; }

    public Planet()
    { }
    public Planet(string name)
    {
      Designation = name;
    }
  }
}
