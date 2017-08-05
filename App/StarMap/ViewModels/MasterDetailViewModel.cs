using Prism.Navigation;
using Prism.Services;
using StarMap.ViewModels.Core;

namespace StarMap.ViewModels
{
  public class MasterDetailViewModel : Navigator
  {
    public MasterDetailViewModel(INavigationService navigationService, IPageDialogService pageDialogService) 
      : base(navigationService, pageDialogService) { }
  }
}
