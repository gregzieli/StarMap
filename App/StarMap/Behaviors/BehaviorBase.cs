using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StarMap.Behaviors
{
  // Taken from https://blog.xamarin.com/turn-events-into-commands-with-behaviors/
  public class BehaviorBase<T> : Behavior<T> where T : BindableObject
  {
    protected override void OnAttachedTo(T bindable)
    {
      base.OnAttachedTo(bindable);
    }

    protected override void OnDetachingFrom(T bindable)
    {
      base.OnDetachingFrom(bindable);
    }
  }
}
