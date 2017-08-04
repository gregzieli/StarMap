using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using StarMap.Bll.Helpers;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Abstractions.Services;
using StarMap.Cll.Constants;
using StarMap.Cll.Exceptions;
using StarMap.Cll.Filters;
using StarMap.Cll.Models.Core;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Abstractions;
using StarMap.Core.Extensions;
using StarMap.Core.Models;
using StarMap.Urho;
using StarMap.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Urho;
using Urho.Forms;

namespace StarMap.ViewModels
{
  // TODO: maybe not another parent, but make this class into partial classes, there's way too much code in here for me.
  public class MainPageViewModel : StarGazer<Universe, UniverseUrhoException>, IApplicationLifecycle
  {
    IDeviceRotation _motionDetector;

    static IReferencable Earth;

    private IReferencable _currentPosition;
    public IReferencable CurrentPosition
    {
      get { return _currentPosition; }
      set { SetProperty(ref _currentPosition, value); }
    }

    private ObservantCollection<Constellation> _constellations;
    public ObservantCollection<Constellation> Constellations
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
    
    private Constellation _selectedConstellation;
    public Constellation SelectedConstellation
    {
      get { return _selectedConstellation; }
      set { SetProperty(ref _selectedConstellation, value, () => OnConstellationSelected(value)); }
    }

    private DelegateCommand _resetFiltersCommand;
    public DelegateCommand ResetFiltersCommand =>
        _resetFiltersCommand ?? (_resetFiltersCommand = new DelegateCommand(ResetFilter));

    // Setting individual switches should be handled maybe:
    // extending the model here with an INotifyPropChanged implementation
    // that could add/remove the value to some collection in this VM
    // and each time that collection changes, GetStars is called with the filtered constellations.
    private DelegateCommand<object> _filterConstellationsCommand;
    public DelegateCommand<object> FilterConstellationsCommand =>
      // T could not be bool, weird.
        _filterConstellationsCommand ?? (_filterConstellationsCommand = new DelegateCommand<object>(FilterConstellations, x => CanExecute()));

    private DelegateCommand _getStarsCommand;
    public DelegateCommand GetStarsCommand =>
        _getStarsCommand ?? (_getStarsCommand = new DelegateCommand(GetStars));

    public DelegateCommand SelectStarCommand { get; private set; }
    public DelegateCommand ShowStarDetailsCommand { get; private set; }

    private DelegateCommand _travelCommand;
    public DelegateCommand TravelCommand =>
        _travelCommand ?? (_travelCommand = new DelegateCommand(Travel, () => SelectedStar != null).ObservesProperty(() => SelectedStar));

    private DelegateCommand _goHomeCommand;
    public DelegateCommand GoHomeCommand =>
        _goHomeCommand ?? (_goHomeCommand = new DelegateCommand(GoHome));

    private async void GoHome()
    {
      await CallAsync(UrhoApplication.GoHome);
      CurrentPosition = Earth;
      Settings.Astrolocation = CurrentPosition.Id;
    }

    private async void Travel()
    {
      var target = SelectedStar;
      await CallAsync(() => UrhoApplication.Travel(target));
      CurrentPosition = target;
      Settings.Astrolocation = CurrentPosition.Id;
    }

    public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IStarManager starManager, IDeviceRotation motionDetector) 
      : base(navigationService, pageDialogService, starManager)
    {
      _motionDetector = motionDetector;
      Earth = new Planet("Earth");

      //SelectStarCommand = new DelegateCommand(SelectStar);
      ShowStarDetailsCommand = new DelegateCommand(ShowStarDetails, () => SelectedStar != null)
           // Cannot use ObservesCanExecute extension here because it observes a bool property only, but it's OK to use ObservesProperty
           // (That way I dont have to ShowStarDetailsCommand.RaiseCanExecuteChanged() in the SelectedStar setter)
           // And it's fluent, and u can observe as many props as u want
           .ObservesProperty(() => SelectedStar);
    }

    #region methods

    private async void ShowStarDetails()
    {
      //Another option:
      //Navigate($"StarDetailPage?id={SelectedStar.Id}");
      //=> await Navigate("StarDetailPage", "id", SelectedStar.Id);
      await Navigate(new Uri(Navigation.DetailAbsolute, UriKind.Absolute), Navigation.Keys.StarId, SelectedStar.Id);
    }

    private async void OnConstellationSelected(Constellation c)
    {
      await Call(() =>
      {
        if (c == null)
          UrhoApplication.ResetHighlight();
        else
        {
          var selection = VisibleStars.Where(x => x.ConstellationId == c.Id);
          UrhoApplication.HighlightStars(selection);
        }
      });      
    }

    async Task Foo(IEnumerable<IUnique> stars, bool turnOn)
    {
      await CallAsync(() => Application.InvokeOnMainAsync(() =>
      {
        UrhoApplication.ToggleConstellations(stars, turnOn);
      }));
    }

    async void OnConstellationFiltered(object sender, PropertyChangedEventArgs e)
    {
      Constellation c = sender as Constellation;
      Debug.WriteLine($"{c.Name} is {(c.IsOn ? "visible" : "hidden")}");
      await Foo(VisibleStars.Where(x => x.ConstellationId == c.Id), c.IsOn);
    }

    async void FilterConstellations(object command)
    {
      // Unsubscribe to avoid 80-something consecutive calls
      Constellations.ElementChanged -= OnConstellationFiltered;
      bool action = (bool)command;
      foreach (var c in Constellations)
        c.IsOn = action;
      await Foo(VisibleStars.Where(x => x.ConstellationId.HasValue), action);

      // Subscribe back on
      Constellations.ElementChanged += OnConstellationFiltered;
    }

    private async void GetStars()
    {
      await CallAsync(async () =>
      {
        await GetStarsFromDatabase();
        await UpdateUrho();
      });
    }

    async Task GetStarsFromDatabase()
    {
      var stars = await _starManager.GetStarsAsync(StarFilter).ConfigureAwait(false);
      // Since the size of the collection may differ, it's better memorywise to instantiate a new one,
      // rather than reuse the already allocated list with a completely different size.
      VisibleStars = new ObservableCollection<Star>(stars);
    }

    async Task UpdateUrho()
    {
      var star = VisibleStars.FirstOrDefault(x => x.Id == Settings.Astrolocation);
      if (star != null && star.Constellation is null && star.ConstellationId != null)
        star.Constellation = Constellations.First(x => x.Id == star.ConstellationId);

      CurrentPosition = star ?? Earth;
      await Application.InvokeOnMainAsync(() => UrhoApplication.UpdateWithStars(VisibleStars, CurrentPosition));
    }

    async Task GetConstellations()
    {
      var constellations = await _starManager.GetConstellationsAsync();
      Constellations = new ObservantCollection<Constellation>(constellations);
      Constellations.ElementChanged += OnConstellationFiltered;
    }

    private void ResetFilter()
    {
      //FilterConstellations(true);
      SelectedStar = null;
      Settings.Filter = null;
      StarFilter = new StarFilter();
      GetStars();
    }
    #endregion



    

    protected override async Task Restore(NavigationParameters parameters)
    {
      SensorStart();

      if (Constellations != null)
        return;

      await CallAsync(() =>
      {
        StarFilter = _starManager.LoadFilter();

        return Task.WhenAll(GetConstellations(), GetStarsFromDatabase(), base.Restore(parameters));
      });
    }    

    protected override async Task CleanUp()
    {
      await CallAsync(() =>
      {
        SensorStop();
        Constellations.ElementChanged -= OnConstellationFiltered;
        Constellations.Clear();
        Constellations = null;
        VisibleStars.Clear();
        VisibleStars = null;
        return base.CleanUp();
      });
    }

    private void OnRotationChanged(object sender, RotationChangedEventArgs e)
    {
      //Debug.WriteLine($"{e.Azimuth}, {e.Pitch}, {e.Roll}");
    }

    public void OnResume()
    {
      SensorStart();
    }

    public void OnSleep()
    {
      SensorStop();
    }

    void SensorStart()
    {
      if (Settings.SensorsOn)
      {
        _motionDetector.Start();
        _motionDetector.RotationChanged += OnRotationChanged;
      }
    }

    void SensorStop()
    {
      if (Settings.SensorsOn)
      {
        _motionDetector.Stop();
        _motionDetector.RotationChanged -= OnRotationChanged;
      }
    }

    public override async Task OnUrhoGenerated()
    {
      UrhoApplication.Input.TouchEnd += SelectStar;
      await UpdateUrho();
    }

    private async void SelectStar(TouchEndEventArgs obj)
    {
      await Call(() =>
      {
        var id = UrhoApplication.OnTouched(obj, out float relativeDistance);
        if (!id.IsNullOrEmpty())
        {
          var star = VisibleStars.FirstOrDefault(x => x.Id == int.Parse(id));
          // NB: I could set refs to constellations for all the stars upon retrieving from db,
          //     it's the obvious thing to do. But, in most operations I need only the Id. 
          //     The Constellation ref is needed only to display proper designation.
          //     And no one is going to select every star there is. That's why i decided to get the constellation ref
          //     here, which can be misleading.
          
          if (star.Constellation is null && star.ConstellationId != null)
            star.Constellation = Constellations.First(x => x.Id == star.ConstellationId);

          star.RelativeDistance = relativeDistance;
          SelectedStar = star;
        }
        else
        {
          SelectedStar = null;
        }
      });      
    }
  }
}
