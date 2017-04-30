using System;
using Prism.Commands;
using Prism.Navigation;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Models;
using System.Collections.ObjectModel;
using Prism.Events;
using StarMap.Events;
using StarMap.ViewModels.Core;
using System.Threading.Tasks;
using Prism.Services;
using StarMap.Cll.Models.Cosmos;

namespace StarMap.ViewModels
{
  public class MainPageViewModel : Navigator
  {
    IStarManager _starManager;

    private Star _selectedStar;
    public Star SelectedStar
    {
      get { return _selectedStar; }
      set { SetProperty(ref _selectedStar, value); }
    }

    private ObservableCollection<Star> _visibleStars;
    public ObservableCollection<Star> VisibleStars
    {
      get { return _visibleStars; }
      set { SetProperty(ref _visibleStars, value); }
    }

    public DelegateCommand SelectStarCommand { get; private set; }    
    public DelegateCommand ShowStarDetailsCommand { get; private set; }

    public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IEventAggregator eventAggregator, IStarManager starManager) 
      : base(navigationService, pageDialogService)
    {
      _starManager = starManager;
      // TODO: maybe move to OnNavigat[ed/ing]To
      VisibleStars = new ObservableCollection<Star>(
        _starManager.GetStars(
          new Cll.Filters.StarFilter() { Limit = 100, MaxDistance = 7000 }));

      SelectStarCommand = new DelegateCommand(SelectStar);
      ShowStarDetailsCommand = new DelegateCommand(ShowStarDetails, () => SelectedStar != null)
           // Cannot use ObservesCanExecute extension here, but it's OK to use ObservesProperty
           // (That way I dont have to ShowStarDetailsCommand.RaiseCanExecuteChanged() in the SelectedStar setter)
           // And it's fluent, and u can observe as many props as u want
           .ObservesProperty(() => SelectedStar);

      // For publish, we want to publish on a scenarion: not in constructor. 
      // Subscribing in a constructor means we dont have to store the aggregator in a field.
      eventAggregator.GetEvent<MyEvent>().Subscribe((payload) => { /*TODO: do sth*/});

    }

    private async void ShowStarDetails()
    {
      await Navigate("StarDetailPage", SelectedStar.Id);
      //Another option
      //Navigate($"StarDetailPage?id={SelectedStar.Id}");
    }

    private void SelectStar()
    {
      if (SelectedStar == null)
        SelectedStar = VisibleStars[new Random().Next(VisibleStars.Count)];
      else
        SelectedStar = null;
    }
  }
}
