using Prism.Commands;
using Prism.Mvvm;
using StarMap.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using Prism.Services;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Models.Cosmos;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StarMap.ViewModels
{
  public class TestOverlayPageViewModel : StarGazer
  {
    private ObservableCollection<Constellation> _constellations;
    public ObservableCollection<Constellation> Constellations
    {
      get { return _constellations; }
      set { SetProperty(ref _constellations, value); }
    }


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

    //  give this one a different color.
    private Constellation _selectedConstellation;
    public Constellation SelectedConstellation
    {
      get { return _selectedConstellation; }
      set { SetProperty(ref _selectedConstellation, value, () => ConstellationSelected(value)); }
    }

    // Setting individual switches should be handled maybe:
    // extending the model here with an INotifyPropChanged implementation
    // that could add/remove the value to some collection in this VM
    // and each time that collection changes, GetStars is called with the filtered constellations.
    private DelegateCommand<string> _showClearConstellationsCommand;
    public DelegateCommand<string> ShowClearConstellationsCommand =>
        _showClearConstellationsCommand ?? (_showClearConstellationsCommand = new DelegateCommand<string>(ShowClearConstellations));

    public DelegateCommand SelectStarCommand { get; private set; }
    public DelegateCommand ShowStarDetailsCommand { get; private set; }

    public TestOverlayPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IStarManager starManager) : base(navigationService, pageDialogService, starManager)
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

    private async void ShowStarDetails()
      //Another option:
      //Navigate($"StarDetailPage?id={SelectedStar.Id}");
      => await Navigate("StarDetailPage", "TODO", SelectedStar.Id);

    private void SelectStar()
    {
      if (SelectedStar == null)
        SelectedStar = VisibleStars[new Random().Next(VisibleStars.Count)];
      else
        SelectedStar = null;
    }

    private void ConstellationSelected(Constellation c)
    {
      Debug.WriteLine(c?.Name ?? "null");
    }

    private void ShowClearConstellations(string command)
    {
      bool action = command == "show";
      foreach (var c in Constellations)
        c.IsSelected = action;
    }

    protected override async Task Restore()
    {
      if (Constellations != null)
        return;

      await Call(() =>
      {
        var constellations = StarManager.GetConstellations();
        Constellations = new ObservableCollection<Constellation>(constellations);
      });
    }
  }
}
