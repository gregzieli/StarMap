using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using System.Collections.ObjectModel;
using StarMap.Cll.Models;
using StarMap.ViewModels.Core;
using System.Threading.Tasks;
using Prism.Services;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Models.Cosmos;

namespace StarMap.ViewModels
{
  public class MasterDetailViewModel : StarGazer
  {
    #region members
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
      set { SetProperty(ref _selectedConstellation, value); }
    }

    private bool _constellationsVisible;
    public bool ConstellationsVisible
    {
      get { return _constellationsVisible; }
      set { SetProperty(ref _constellationsVisible, value); }
    }
    #endregion

    #region commands
    private DelegateCommand _showHideConstellationMenuCommand;
    public DelegateCommand ShowHideConstellationMenuCommand =>
        _showHideConstellationMenuCommand ?? (_showHideConstellationMenuCommand = new DelegateCommand(ShowHideConstellationMenu));


    private DelegateCommand<string> _showClearConstellationsCommand;
    public DelegateCommand<string> ShowClearConstellationsCommand =>
        _showClearConstellationsCommand ?? (_showClearConstellationsCommand = new DelegateCommand<string>(ShowClearConstellations));

    #endregion

    private void ShowClearConstellations(string command)
    {
      bool action = command == "show";
      foreach (var c in Constellations)
        c.IsSelected = action;
    }

    private void ShowHideConstellationMenu()
    {
      var isVisible = !ConstellationsVisible;
      ConstellationsVisible = isVisible;

      if (!isVisible)
        SelectedConstellation = null;
    }

    public MasterDetailViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IStarManager starManager) 
      : base(navigationService, pageDialogService, starManager)
    {
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



      // Since the SQLiteConnection I'm using is not async, the call shouldn't be async as well.
      // TODO: remove
      //await CallAsync(async () =>
      //{
      //  if (Constellations != null)
      //    return;
      //  var constellations = await Task.Run(() => StarManager.GetConstellations()).ConfigureAwait(false);
      //  Constellations = new ObservableCollection<Constellation>(constellations);
      //});
    }
  }
}
