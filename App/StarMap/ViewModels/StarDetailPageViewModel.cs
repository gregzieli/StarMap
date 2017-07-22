using Prism.Navigation;
using Prism.Services;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Constants;
using StarMap.Cll.Exceptions;
using StarMap.Cll.Models.Cosmos;
using StarMap.Urho;
using StarMap.ViewModels.Core;
using System.Threading.Tasks;

namespace StarMap.ViewModels
{
  public class StarDetailPageViewModel : StarGazer<SingleStar, StarDetailUrhoException>
  {
    public StarDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IStarManager starManager)
       : base(navigationService, pageDialogService, starManager)
    { }

    private StarDetail _star;
    public StarDetail Star
    {
      get => _star;
      set { SetProperty(ref _star, value); }
    }
    
    protected override async Task Restore(NavigationParameters parameters)
    {
      //await base.Restore(parameters); // TODO: check if this was causing the errors
      await CallAsync(() => _starManager.GetStarDetailsAsync((int)parameters[Navigation.Keys.StarId]), star => Star = star);
    }

    public override async Task OnUrhoGenerated()
      => UrhoApplication.SetStar(Star);
  }
}
