using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace StarMap.ViewModels.Core
{
  public abstract class Navigator : Interlocutor, INavigationAware
  {
    INavigationService _navigationService;

    public virtual DelegateCommand<string> NavigateCommand { get; private set; }

    public Navigator(INavigationService navigationService, IPageDialogService pageDialogService)
      : base(pageDialogService)
    {
      _navigationService = navigationService;
      NavigateCommand = new DelegateCommand<string>(Navigate);
    }

    protected async void Navigate(string uri)
    {
      await Navigate(uri, null);
    }

    protected async Task Navigate(string uri, object param)
    {
      var navParams = new NavigationParameters() { { "TODO", param } };
      await Navigate(uri, navParams);
    }

    protected async Task Navigate(string uri, NavigationParameters navParams)
    {
      await CallAsync(() => _navigationService.NavigateAsync(uri, navParams));
    }

    protected async Task GoBack()
    {
      await CallAsync(() => _navigationService.GoBackAsync());
    }

    public virtual void OnNavigatedFrom(NavigationParameters parameters)
    { }

    public virtual void OnNavigatedTo(NavigationParameters parameters)
    { }

    public virtual async void OnNavigatingTo(NavigationParameters parameters)
    {
      // Check if it works better on NavigatEDTo
      await Restore();
    }

    // Logic to restore VM's properties
    protected virtual async Task Restore()
    { }
  }
}
