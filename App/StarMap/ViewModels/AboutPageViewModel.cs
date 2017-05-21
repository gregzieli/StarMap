using System;
using System.Threading.Tasks;
using Prism.Navigation;
using StarMap.ViewModels.Core;
using Prism.Services;

namespace StarMap.ViewModels
{
  public class AboutPageViewModel : Navigator
  {
    public AboutPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, Cll.Abstractions.Services.IDeviceRotation rotationService) 
      : base(navigationService, pageDialogService)
    {
      _rotationService = rotationService;
    }
    Cll.Abstractions.Services.IDeviceRotation _rotationService;

    private void A_RotationChanged(object sender, StarMap.Core.Models.RotationChangedEventArgs e)
    {
      System.Diagnostics.Debug.WriteLine($"{e.Azimuth}, {e.Pitch}, {e.Roll}");
    }

    protected override async Task Restore()
    {
      await Call(() =>
      {
        _rotationService.Start();
        _rotationService.RotationChanged += A_RotationChanged;
      });
    }
    

    protected override async Task CleanUp()
    {
      await Call(() =>
      {
        _rotationService.Stop();
        _rotationService.RotationChanged -= A_RotationChanged;
      });
    }
  }
}
