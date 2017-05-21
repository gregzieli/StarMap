using System;
using System.Threading.Tasks;
using Prism.Navigation;
using StarMap.ViewModels.Core;
using Prism.Services;
using Prism.AppModel;

namespace StarMap.ViewModels
{
  public class AboutPageViewModel : Navigator
  {
    public AboutPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) 
      : base(navigationService, pageDialogService)
    {
    }    
    
  }
}
