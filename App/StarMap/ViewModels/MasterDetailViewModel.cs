using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Extensions;
using StarMap.Events;
using StarMap.ViewModels.Core;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace StarMap.ViewModels
{
  public class MasterDetailViewModel : StarGazer
  {
    #region members
    IEventAggregator _eventAggregator;

    private ObservableCollection<Constellation> _constellations;
    public ObservableCollection<Constellation> Constellations
    {
      get { return _constellations; }
      set { SetProperty(ref _constellations, value); }
    }

    //  give this one a different color.
    private Constellation _selectedConstellation;
    public Constellation SelectedConstellation
    {
      get { return _selectedConstellation; }
      set { SetProperty(ref _selectedConstellation, value, () => ConstellationSelected(value)); }
    }

    private bool _constellationsVisible;
    public bool ConstellationsVisible
    {
      get { return _constellationsVisible; }
      set { SetProperty(ref _constellationsVisible, value); }
    }
    #endregion

    #region commands

    // Setting individual switches should be handled maybe:
    // extending the model here with an INotifyPropChanged implementation
    // that could add/remove the value to some collection in this VM
    // and each time that collection changes, GetStars is called with the filtered constellations.
    private DelegateCommand<string> _showClearConstellationsCommand;
    public DelegateCommand<string> ShowClearConstellationsCommand =>
        _showClearConstellationsCommand ?? (_showClearConstellationsCommand = new DelegateCommand<string>(ShowClearConstellations));




    //http://prismlibrary.readthedocs.io/en/latest/Xamarin-Forms/6-EventToCommandBehavior/
    // Why bother with this, if the same functionality I get in the SelectedConstellation setter?
    private DelegateCommand<Constellation> _constellationSelectedCommand;
    public DelegateCommand<Constellation> ConstellationSelectedCommand =>
        _constellationSelectedCommand ?? (_constellationSelectedCommand = new DelegateCommand<Constellation>(ConstellationSelected));

    #endregion


    // Async void, because either in the Command handler, or in the setter, can't be awaited.
    // Which is fine, since only the exception handling is async, not the main execution.
    // And main execution is publishing an event, so Void is the proper use here.
    private async void ConstellationSelected(Constellation c)
    {
      await Call(() =>
      {
        _eventAggregator.GetEvent<ConstellationSelectedEvent>().Publish(c);
      });
    }

    private void ShowClearConstellations(string command)
    {
      bool action = command == "show";
      foreach (var c in Constellations)
        c.IsSelected = action;
    }

    public MasterDetailViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IStarManager starManager, IEventAggregator eventAggregator) 
      : base(navigationService, pageDialogService, starManager)
    {
      _eventAggregator = eventAggregator;

      // This is just a shortcut for now
      // Prism doesn't really support the 'Detail' functionality, so I could simply look it up
      // Or does it? Need to investigate
      MainPageActive = !Bll.Helpers.Settings.Geolocation.IsNullOrEmpty();
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
