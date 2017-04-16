using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Models;
using Prism.Events;
using StarMap.Events;
using StarMap.ViewModels.Core;

namespace StarMap.ViewModels
{
  public class StarDetailPageViewModel : Navigator
  {
    IStarManager _starManager;
    private StarDetail _star;
    private IEventAggregator _eventAggregator;

    public StarDetail Star
    {
      get { return _star; }
      set { SetProperty(ref _star, value); }
    }

    public StarDetailPageViewModel(INavigationService navigationService, IStarManager starManager, IEventAggregator eventAggregator) : base(navigationService)
    {
      _starManager = starManager;
      // move this to some base class. For now it's just to show.
      _eventAggregator = eventAggregator;
    }

    public override void OnNavigatingTo(NavigationParameters parameters)
    {
      base.OnNavigatingTo(parameters);
      Star = _starManager.GetStarDetails((int)parameters["TODO"]);

      //TODO: just demo code (remove)
      _eventAggregator.GetEvent<MyEvent>().Publish($"Hello from event {nameof(MyEvent)} publisher");

    }
  }
}
