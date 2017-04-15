using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Models;

namespace StarMap.ViewModels
{
  public class DetailPageViewModel : Navigator
  {
    IStarManager _starManager;
    private StarDetail _star;
    public StarDetail Star
    {
      get { return _star; }
      set { SetProperty(ref _star, value); }
    }

    public DetailPageViewModel(INavigationService navigationService, IStarManager starManager) : base(navigationService)
    {
      _starManager = starManager;
    }

    public override void OnNavigatingTo(NavigationParameters parameters)
    {
      base.OnNavigatingTo(parameters);
      Star = _starManager.GetStarDetails((int)parameters["TODO"]);
    }
  }
}
