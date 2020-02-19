using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace StarMap.ViewModels.Core
{
    public abstract class Navigator : Herald, INavigatedAware
    {
        private readonly INavigationService _navigationService;

        public virtual DelegateCommand<string> NavigateCommand { get; private set; }

        public virtual DelegateCommand GoBackCommand { get; private set; }

        public Navigator(INavigationService navigationService, IPageDialogService pageDialogService)
          : base(pageDialogService)
        {
            _navigationService = navigationService;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack);
        }

        protected async void Navigate(string path)
          => await Navigate(path, null);

        protected async void Navigate(Uri uri)
          => await Navigate(uri, null);

        protected Task Navigate(string path, string key, object param)
        {
            var navParams = new NavigationParameters() { { key, param } };
            return Navigate(path, navParams);
        }

        protected Task Navigate(Uri uri, string key, object param)
        {
            var navParams = new NavigationParameters() { { key, param } };
            return Navigate(uri, navParams);
        }

        protected Task Navigate(string path, INavigationParameters navParams)
          => CallAsync(() => _navigationService.NavigateAsync(path, navParams));

        protected Task Navigate(Uri uri, INavigationParameters navParams)
          => CallAsync(() => _navigationService.NavigateAsync(uri, navParams));

        public async void GoBack()
         => await CallAsync(() => _navigationService.GoBackAsync());

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
          => CleanUp();

        public virtual void OnNavigatedTo(INavigationParameters parameters)
          => Restore(parameters);

        /// <summary>
        /// Logic to restore VM's properties, and other actions done upon opening the page
        /// </summary>
        protected virtual void Restore(INavigationParameters parameters) { }

        /// <summary>
        /// Logic to be executed upon exiting a view model.
        /// </summary>
        protected virtual void CleanUp() { }
    }
}
