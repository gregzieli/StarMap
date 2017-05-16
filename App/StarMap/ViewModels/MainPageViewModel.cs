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
using StarMap.Cll.Constants;
using StarMap.Cll.Filters;

namespace StarMap.ViewModels
{
  public class MainPageViewModel : StarGazer
  {
    private ObservableCollection<Constellation> _constellations;
    public ObservableCollection<Constellation> Constellations
    {
      get { return _constellations; }
      set { SetProperty(ref _constellations, value); }
    }


    private double _distance;
    public double FilteredDistance
    {
      get { return _distance; }
      set { SetProperty(ref _distance, value, () => Debug.WriteLine($"Distance changed to: {value}")); }
    }

    private double _magnitude;
    public double FilteredMagnitude
    {
      get { return _magnitude; }
      set { SetProperty(ref _magnitude, value, () => Debug.WriteLine($"Magnitude changed to: {value}")); }
    }

    private string _designation;
    public string FilteredDesignation
    {
      get { return _designation; }
      set { SetProperty(ref _designation, value, () => Debug.WriteLine($"Designation changed to: {value}")); }
    }

    private Star _selectedStar;
    public Star SelectedStar
    {
      get { return _selectedStar; }
      set { SetProperty(ref _selectedStar, value); }
    }

    private StarFilter _starFilter;
    public StarFilter StarFilter
    {
      get { return _starFilter; }
      set { SetProperty(ref _starFilter, value); }
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

    private DelegateCommand _resetFiltersCommandCommand;
    public DelegateCommand ResetFiltersCommand =>
        _resetFiltersCommandCommand ?? (_resetFiltersCommandCommand = new DelegateCommand(ResetFilter));

    // Setting individual switches should be handled maybe:
    // extending the model here with an INotifyPropChanged implementation
    // that could add/remove the value to some collection in this VM
    // and each time that collection changes, GetStars is called with the filtered constellations.
    private DelegateCommand<object> _showClearConstellationsCommand;
    public DelegateCommand<object> ShowClearConstellationsCommand =>
      // T could not be bool, weird.
        _showClearConstellationsCommand ?? (_showClearConstellationsCommand = new DelegateCommand<object>(ShowClearConstellations));

    private DelegateCommand _getStarsCommand;
    public DelegateCommand GetStarsCommand =>
        _getStarsCommand ?? (_getStarsCommand = new DelegateCommand(GetStars));

    public DelegateCommand SelectStarCommand { get; private set; }
    public DelegateCommand ShowStarDetailsCommand { get; private set; }

    public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IStarManager starManager) : base(navigationService, pageDialogService, starManager)
    {
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

    private void ShowClearConstellations(object command)
    {
      bool action = (bool)command;
      foreach (var c in Constellations)
        c.IsSelected = action;
    }

    private void GetStars()
    {
      var stars = StarManager.GetStars(StarFilter);
      // TODO: verify if newing up is OK, or mabe Clear(), or something else.
      VisibleStars = new ObservableCollection<Star>(stars);

      Debug.WriteLine($"New visible stars, count = {VisibleStars.Count}");
      Debug.WriteLine($"    Mag {StarFilter.MaxMagnitude}");
      Debug.WriteLine($"    Dist {StarFilter.MaxDistance}");
      Debug.WriteLine($"    Name {StarFilter.DesignationQuery}");
    }

    private void ResetFilter()
    {
      ShowClearConstellations(true);
      SelectedStar = null;

      StarFilter = new StarFilter()
      {
        DesignationQuery = null,
        ConstellationsIds = null,
        MaxDistance = Filters.DEF_DIST,
        MaxMagnitude = Filters.DEF_MAG
      };
    }

    protected override async Task Restore()
    {
      if (Constellations != null)
        return;

      await Call(() =>
      {
        var constellations = StarManager.GetConstellations();
        Constellations = new ObservableCollection<Constellation>(constellations);

        ResetFilter();
        GetStars();
      });
    }
  }
}
