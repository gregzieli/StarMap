using Prism.Events;
using System;

namespace StarMap.Cll.Events
{
  public class UrhoErrorEvent<T> : PubSubEvent<T> where T : Exception { }
}
