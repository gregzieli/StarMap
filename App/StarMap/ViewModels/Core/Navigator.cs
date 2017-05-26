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

    public virtual DelegateCommand GoBackCommand { get; private set; }

    public Navigator(INavigationService navigationService, IPageDialogService pageDialogService)
      : base(pageDialogService)
    {
      _navigationService = navigationService;
      NavigateCommand = new DelegateCommand<string>(Navigate);
      GoBackCommand = new DelegateCommand(GoBack);
    }

    public async void GoBack()
     => await CallAsync(() => _navigationService.GoBackAsync());

    protected async void Navigate(string path) 
      => await Navigate(path, null);

    protected async void Navigate(Uri uri)
      => await Navigate(uri, null);   

    protected async Task Navigate(string path, string key, object param)
    {
      var navParams = new NavigationParameters() { { key, param } };
      await Navigate(path, navParams);
    }

    protected async Task Navigate(Uri uri, string key, object param)
    {
      var navParams = new NavigationParameters() { { key, param } };
      await Navigate(uri, navParams);
    }

    protected async Task Navigate(string path, NavigationParameters navParams)
      => await CallAsync(() => _navigationService.NavigateAsync(path, navParams));

    protected async Task Navigate(Uri uri, NavigationParameters navParams)
      => await CallAsync(() => _navigationService.NavigateAsync(uri, navParams));

    public virtual async void OnNavigatedFrom(NavigationParameters parameters)
      => await CleanUp();

    public virtual async void OnNavigatedTo(NavigationParameters parameters)
    {
      // Check if it works better on NavigatINGTo
      // 1. hardware back button calls only this one
      await Restore();
    }

    public virtual async void OnNavigatingTo(NavigationParameters parameters)
    {
      
    }

    // Could make those two abstract to lose the async and the green underline, but then, 
    // since Navigator is direct parent of more than one class, I would have to
    // implement it a couple of times. This is just less code.

    /// <summary>
    /// Logic to restore VM's properties, and other actions done upon opening the page
    /// </summary>
    protected virtual async Task Restore() { }

    /// <summary>
    /// 
    /// Logic to be executed upon exiting a view model.
    /// </summary>
    protected virtual async Task CleanUp() { }
  }
}
