using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarMap.Events
{
  // Prism alternative to Xamarin's Messaging Center
  public class MyEvent : PubSubEvent<string>
  {
  }
}
