using StarMap.Cll.Models.Core;

namespace StarMap.Cll.Models.Cosmos
{
  public class Planet : IReferencable
  {
    public int Id { get; private set; } = -1; // in here just because there is only Earth.

    public string Designation { get; private set; }

    public Planet()
    { }

    public Planet(string name)
    {
      Designation = name;
    }
  }
}
