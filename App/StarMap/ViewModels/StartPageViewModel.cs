using Prism.Navigation;
using StarMap.ViewModels.Core;

namespace StarMap.ViewModels
{
  public class StartPageViewModel : Navigator
  {
    public StartPageViewModel(INavigationService navigationService) : base(navigationService)
    {
    }
  }
}
