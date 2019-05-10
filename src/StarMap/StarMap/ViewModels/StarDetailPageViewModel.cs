using Prism.Navigation;
using Prism.Services;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Constants;
using StarMap.Cll.Exceptions;
using StarMap.Cll.Models.Cosmos;
using StarMap.Urhosharp;
using StarMap.ViewModels.Core;
using System;

namespace StarMap.ViewModels
{
    public class StarDetailPageViewModel : StarGazer<SingleStar, StarDetailUrhoException>
    {
        public StarDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IStarManager starManager)
           : base(navigationService, pageDialogService, starManager) { }

        private StarDetail _star;
        public StarDetail Star
        {
            get => _star;
            set { SetProperty(ref _star, value); }
        }

        protected override async void Restore(INavigationParameters parameters)
        {
            base.Restore(parameters);
            await CallAsync(() =>
              StarManager.GetStarDetailsAsync(Convert.ToInt32(parameters[Navigation.Keys.StarId])),
              star =>
                Star = star);
        }

        public override void OnUrhoGenerated()
          => UrhoApplication.SetStar(Star);
    }
}
