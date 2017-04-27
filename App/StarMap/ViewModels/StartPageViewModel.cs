using System;
using System.Threading.Tasks;
using Prism.Navigation;
using StarMap.ViewModels.Core;
using Prism.Services;

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
