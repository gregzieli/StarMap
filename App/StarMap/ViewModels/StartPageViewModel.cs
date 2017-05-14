using System;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using StarMap.ViewModels.Core;
using StarMap.Bll.Helpers;
using StarMap.Core.Extensions;

namespace StarMap.ViewModels
{
  public class StartPageViewModel : Navigator
  {
    public StartPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) 
      : base(navigationService, pageDialogService)
    {
    }

    //private DelegateCommand _startCommand;
    //public DelegateCommand StartCommand =>
    //    _startCommand ?? (_startCommand = new DelegateCommand(Start));

    //void Start()
    //{
    //  var p = Settings.Geolocation;
    //  if (p.IsNullOrEmpty())
    //    Navigate("MasterDetail/NavigationPage/SettingsPage");
    //  else
    //    Navigate("MasterDetail/TestOverlayPage");
    //}
  }
}
