using Prism.Navigation;
using Prism.Services;
using StarMap.Cll.Constants;
using StarMap.ViewModels.Core;

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
            set => SetProperty(ref _url, value);
        }

        protected override void Restore(INavigationParameters parameters)
        {
            Url = parameters[Navigation.Keys.Url] as string;
        }
    }
}
