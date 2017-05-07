using Prism.Events;
using StarMap.Cll.Models.Cosmos;

namespace StarMap.Events
{
  public class ConstellationSelectedEvent : PubSubEvent<Constellation> { }
}
