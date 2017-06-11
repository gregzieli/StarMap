using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using Prism.Events;

namespace StarMap.ViewModels.Core
{
  public abstract class Listener : Navigator
  {
    IEventAggregator _eventAggregator;
    public Listener(INavigationService navigationService, IPageDialogService pageDialogService, IEventAggregator ea) : base(navigationService, pageDialogService)
    {
      _eventAggregator = ea;
    }

    // Again, this is too much, totaly counter-productive.
    protected void Foo<T, TT>() where T : PubSubEvent<TT>, new()
    {
      //_eventAggregator.GetEvent<T>().Subscribe()
    }
  }
}
