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
      // I dont like how this logicis handled here...
      // But I wanted to avoid using some singleton to keep state for IsMainPage across VMs
      // TODO: CONSTANTS!!!!!!

      // TODO: Actually, move this to MasterDetailVM, just to navigate to main page use a dedicated command, not this global one.
      MainPageActive = uri == "MainPage";
      await Navigate(uri, null);
    }

    protected async Task Navigate(string uri, string key, object param)
    {
      var navParams = new NavigationParameters() { { key, param } };
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

    public virtual async void OnNavigatedFrom(NavigationParameters parameters)
    {
      await CleanUp();
    }

    public virtual async void OnNavigatedTo(NavigationParameters parameters)
    {
      // Check if it works better on NavigatINGTo
      // 1. hardware back button calls only this one
      await Restore();
    }

    public virtual async void OnNavigatingTo(NavigationParameters parameters)
    {
      
    }

    /// <summary>
    /// Logic to restore VM's properties, and other actions done upon opening the page
    /// </summary>
    protected virtual async Task Restore() { }

    /// <summary>
    /// 
    /// Logic to be executed upon exiting a view model.
    /// </summary>
    protected virtual async Task CleanUp() { }

    private bool _mainPageActive;
    public bool MainPageActive
    {
      get { return _mainPageActive; }
      set { SetProperty(ref _mainPageActive, value); }
    }
  }
}
