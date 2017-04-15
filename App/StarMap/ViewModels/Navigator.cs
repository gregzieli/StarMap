using Prism.Commands;
using Prism.Navigation;

namespace StarMap.ViewModels
{
  public class Navigator : BaseViewModel, INavigationAware
  {
    INavigationService _navigationService;

    public virtual DelegateCommand<string> NavigateCommand { get; set; }

    private bool _isBusy = false;
    public bool IsBusy
    {
      get { return _isBusy; }
      set { SetProperty(ref _isBusy, value); }
    }

    public Navigator(INavigationService navigationService)
    {
      _navigationService = navigationService;
      NavigateCommand = new DelegateCommand<string>(Navigate);
    }

    protected async void Navigate(string uri)
    {
      await _navigationService.NavigateAsync(uri);
    }

    protected async void Navigate(string uri, object param)
    {
      var navParams = new NavigationParameters();
      await _navigationService.NavigateAsync(uri, new NavigationParameters() { { "TODO", param } });
    }

    protected async void Navigate(string uri, NavigationParameters navParams)
    {
      await _navigationService.NavigateAsync(uri, navParams);
    }

    protected async void GoBack()
    {
      await _navigationService.GoBackAsync();
    }

    public virtual void OnNavigatedFrom(NavigationParameters parameters)
    { }

    public virtual void OnNavigatedTo(NavigationParameters parameters)
    { }

    public virtual void OnNavigatingTo(NavigationParameters parameters)
    { }
  }
}
