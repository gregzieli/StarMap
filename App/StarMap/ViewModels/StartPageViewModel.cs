using Prism.Navigation;
using Prism.Services;
using StarMap.ViewModels.Core;

namespace StarMap.ViewModels
{
  public class StartPageViewModel : Navigator
  {
    public StartPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) 
      : base(navigationService, pageDialogService)
    {
    }
  }
}
