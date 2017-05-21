using Prism.Navigation;
using Prism.Services;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Models.Cosmos;
using StarMap.ViewModels.Core;

namespace StarMap.ViewModels
{
  public class StarDetailPageViewModel : Navigator
  {
    IStarManager _starManager;

    private StarDetail _star;
    public StarDetail Star
    {
      get { return _star; }
      set { SetProperty(ref _star, value); }
    }

    public StarDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IStarManager starManager) 
      : base(navigationService, pageDialogService)
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
