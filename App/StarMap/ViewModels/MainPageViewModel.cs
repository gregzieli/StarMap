using System;
using Prism.Commands;
using Prism.Navigation;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Models;
using System.Collections.ObjectModel;
using Prism.Events;
using StarMap.ViewModels.Core;
using System.Threading.Tasks;
using Prism.Services;
using StarMap.Cll.Models.Cosmos;
using System.Diagnostics;
using StarMap.Events;

namespace StarMap.ViewModels
{
  public class MainPageViewModel : StarGazer
  {
    IEventAggregator _eventAggregator;

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
      : base(navigationService, pageDialogService, starManager)
    {
      // TODO: maybe move to OnNavigat[ed/ing]To
      VisibleStars = new ObservableCollection<Star>(
        StarManager.GetStars(
          new Cll.Filters.StarFilter() { Limit = 100, MaxDistance = 7000 }));

      SelectStarCommand = new DelegateCommand(SelectStar);
      ShowStarDetailsCommand = new DelegateCommand(ShowStarDetails, () => SelectedStar != null)
           // Cannot use ObservesCanExecute extension here, but it's OK to use ObservesProperty
           // (That way I dont have to ShowStarDetailsCommand.RaiseCanExecuteChanged() in the SelectedStar setter)
           // And it's fluent, and u can observe as many props as u want
           .ObservesProperty(() => SelectedStar);
    }

    void HandleConstellationRequest(Constellation constellation)
    {

      Debug.WriteLine($"Selected {constellation?.Name ?? "null"}");
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

    protected override async Task Restore()
    {
      await Call(() =>
      {
        _eventAggregator.GetEvent<ConstellationSelectedEvent>().Subscribe(HandleConstellationRequest);
      });
    }
  }
}
