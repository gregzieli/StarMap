using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using StarMap.Bll.Helpers;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Abstractions.Services;
using StarMap.Cll.Constants;
using StarMap.Cll.Exceptions;
using StarMap.Cll.Filters;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Abstractions;
using StarMap.Core.Extensions;
using StarMap.Core.Models;
using StarMap.Urho;
using StarMap.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Urho;

namespace StarMap.ViewModels
{
  public class MainPageViewModel : StarGazer<Universe, UniverseUrhoException>
  {
    IDeviceRotation _motionDetector;    
    IUnique _sol;

    #region Properties

    protected override bool CanExecute => !_loading;

    public IList<Star> Stars { get; set; }

    private bool _loading = true;
    public bool Loading
    {
      get { return _loading; }
      set { SetProperty(ref _loading, value); }
    }

    private ObservantCollection<Constellation> _constellations;
    public ObservantCollection<Constellation> Constellations
    {
      get { return _constellations; }
      set { SetProperty(ref _constellations, value); }
    }

    private StarFilter _starFilter;
    public StarFilter StarFilter
    {
      get { return _starFilter; }
      set { SetProperty(ref _starFilter, value); }
    }

    private IUnique _currentPosition;
    public IUnique CurrentPosition
    {
      get { return _currentPosition; }
      set { SetProperty(ref _currentPosition, value); }
    }

    private Star _selectedStar;
    public Star SelectedStar
    {
      get { return _selectedStar; }
      set { SetProperty(ref _selectedStar, value); }
    }

    private Constellation _selectedConstellation;
    public Constellation SelectedConstellation
    {
      get { return _selectedConstellation; }
      set { SetProperty(ref _selectedConstellation, value, () => OnConstellationSelected(value)); }
    }

    #endregion

    #region Commands

    // T could not be bool, weird.
    public DelegateCommand<object> FilterConstellationsCommand { get; private set; }
    
    public DelegateCommand ResetFiltersCommand { get; private set; }
    
    public DelegateCommand GetStarsCommand { get; private set; }

    public DelegateCommand ShowStarDetailsCommand { get; private set; }
    
    public DelegateCommand TravelCommand { get; private set; }
    
    public DelegateCommand GoHomeCommand { get; private set; }

    #endregion

    public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IStarManager starManager, IDeviceRotation motionDetector)
      : base(navigationService, pageDialogService, starManager)
    {
      _motionDetector = motionDetector;

      SetCommands();
    }

    void SetCommands()
    {
      // Cannot use ObservesCanExecute extension here because it observes a bool property only, but it's OK to use ObservesProperty
      // (That way I dont have to ShowStarDetailsCommand.RaiseCanExecuteChanged() in the SelectedStar setter)
      // And it's fluent, and u can observe as many props as u want
      ShowStarDetailsCommand = new DelegateCommand(GoToDetails, () => SelectedStar != null)
        .ObservesProperty(() => SelectedStar);

      TravelCommand = new DelegateCommand(() => Travel(SelectedStar), () => SelectedStar != null)
        .ObservesProperty(() => SelectedStar);

      GoHomeCommand = new DelegateCommand(() => Travel(_sol));

      FilterConstellationsCommand = new DelegateCommand<object>(FilterConstellations)
        .ObservesCanExecute(() => CanExecute);

      ResetFiltersCommand = new DelegateCommand(ResetFilter)
        .ObservesCanExecute(() => CanExecute);

      GetStarsCommand = new DelegateCommand(ShowStars)
        .ObservesCanExecute(() => CanExecute);
    }
    
    #region Command methods

    async void GoToDetails()
    {
      await Navigate(new Uri(Navigation.DetailAbsolute, UriKind.Absolute), Navigation.Keys.StarId, SelectedStar.Id);

      // modal causes errors. 
      //await Navigate("StarDetailPage", Navigation.Keys.StarId, SelectedStar.Id);
    }

    async void ShowStars()
    {
      await CallAsync(async () =>
      {
        if (!_starManager.CheckFilterChanged(StarFilter))
          return;

        Loading = true;
        SelectedStar = null;

        await GetStars();
        await UpdateUrho();
      }, always: () => Loading = false);
    }
    
    async void Travel(IUnique target)
    {
      await CallAsync(() => UrhoApplication.Travel(target),
        onDone: () =>
        {
          CurrentPosition = target;
          Settings.Astrolocation = CurrentPosition.Id;
        });
    }

    async void FilterConstellations(object command)
    {
      await CallAsync(() =>
      {
        // Unsubscribe to avoid 80-something consecutive calls
        Constellations.ElementChanged -= OnConstellationFiltered;
        bool action = (bool)command;
        foreach (var c in Constellations)
          c.IsOn = action;

        return ToggleConstellations(Stars.Where(x => x.ConstellationId.HasValue), action);
      }, onDone: () => // Subscribe back on
        Constellations.ElementChanged += OnConstellationFiltered);
    }

    void ResetFilter()
    {
      // Execute only if there was a change
      StarFilter.Reset();
      ShowStars();
    }


    #endregion

    #region Event Handlers

    protected override async void Restore(NavigationParameters parameters)
    {
      await CallAsync(() =>
      {
        base.Restore(parameters);

        StartSensors();

        StarFilter = _starManager.LoadFilter();

        return GetConstellations();
      });
    }

    protected override void CleanUp()
    {
      StopSensors();
      Constellations.ElementChanged -= OnConstellationFiltered;
      base.CleanUp();
    }

    public override void OnResume()
    {
      base.OnResume();
      StartSensors();
    }

    public override void OnSleep()
    {
      base.OnSleep();
      StopSensors();
    }

    async void OnConstellationSelected(Constellation c)
    {
      await Call(() =>
      {
        if (c == null)
          UrhoApplication.ResetHighlight();
        else
        {
          var selection = Stars.Where(x => x.ConstellationId == c.Id);
          UrhoApplication.HighlightStars(selection);
        }
      });      
    }

    async void OnConstellationFiltered(object sender, PropertyChangedEventArgs e)
    {
      await CallAsync(() =>
      {
        var c = (Constellation)sender;
        return ToggleConstellations(Stars.Where(x => x.ConstellationId == c.Id), c.IsOn);
      });
    }

    public override async void OnUrhoGenerated()
    {
      await CallAsync(async () =>
      {
        UrhoApplication.Input.TouchEnd += OnStarSelected;
        await GetStars();
        await UpdateUrho();
      }, always: () => Loading = false);
    }

    void OnRotationChanged(object sender, RotationChangedEventArgs e)
    {
      UrhoApplication?.SetRotation(e.Orientation);
    }

    async void OnStarSelected(TouchEndEventArgs obj)
    {
      await Call(() =>
      {
        var id = UrhoApplication.OnTouched(obj, out float relativeDistance);
        if (!id.IsNullOrEmpty())
        {
          var star = Stars.FirstOrDefault(x => x.Id == int.Parse(id));
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

    #endregion

    Task ToggleConstellations(IEnumerable<IUnique> stars, bool turnOn)
    {
      return Application.InvokeOnMainAsync(() =>
      {
        UrhoApplication.ToggleConstellations(stars, turnOn);
      });
    }

    async Task GetConstellations()
    {
      var constellations = await _starManager.GetConstellationsAsync().ConfigureAwait(false);
      Constellations = new ObservantCollection<Constellation>(constellations);
      Constellations.ElementChanged += OnConstellationFiltered;
    }

    async Task GetStars()
    {
      var stars = await _starManager.GetStarsAsync(StarFilter).ConfigureAwait(false);
      // Since the size of the collection may differ, it's better memorywise to instantiate a new one,
      // rather than reuse the already allocated list with a completely different size.
      Stars = new List<Star>(stars);

      if (_sol is null)
        _sol = stars.First(x => x.Id == 0);
    }

    async Task UpdateUrho()
    {
      var currentLocation = Stars.FirstOrDefault(x => x.Id == Settings.Astrolocation);
      if (currentLocation != null && currentLocation.Constellation is null && currentLocation.ConstellationId != null)
        currentLocation.Constellation = Constellations.First(x => x.Id == currentLocation.ConstellationId);

      CurrentPosition = currentLocation ?? _sol;
      await Application.InvokeOnMainAsync(() => UrhoApplication.UpdateWithStars(Stars, CurrentPosition));
    }

    void StartSensors()
    {
      if (Settings.SensorsOn)
      {
        _motionDetector.Start();
        _motionDetector.RotationChanged += OnRotationChanged;
      }
    }

    void StopSensors()
    {
      if (Settings.SensorsOn)
      {
        _motionDetector.Stop();
        _motionDetector.RotationChanged -= OnRotationChanged;
      }
    }
  }
}
