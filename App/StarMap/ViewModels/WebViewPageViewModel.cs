using Prism.Navigation;
using Prism.Services;
using StarMap.ViewModels.Core;
using System.Threading.Tasks;

namespace StarMap.ViewModels
{
  public class WebViewPageViewModel : Navigator
  {
    public WebViewPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) 
      : base(navigationService, pageDialogService)
    {
    }

    private string _url;
    public string Url
    {
      get => _url;
      set { SetProperty(ref _url, value); }
    }

    protected override async Task Restore(NavigationParameters parameters)
    {
      Url = parameters["url"] as string;
    }
  }
}
