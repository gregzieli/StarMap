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
  public class MasterDetailViewModel : Navigator
  {
    #region Just for show - no need for it here, nor anywhere in the application for now
    // Two separate functionalities here: 
    // 1. Use of PubSubEvents
    // 2. EventToCommandBehavior
    IEventAggregator _eventAggregator;

    //http://prismlibrary.readthedocs.io/en/latest/Xamarin-Forms/6-EventToCommandBehavior/
    // Why bother with this, if the same functionality I get in the SelectedConstellation setter?
    private DelegateCommand<Constellation> _constellationSelectedCommand;
    public DelegateCommand<Constellation> ConstellationSelectedCommand =>
        _constellationSelectedCommand ?? (_constellationSelectedCommand = new DelegateCommand<Constellation>(ConstellationSelected));

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

    #endregion

    public MasterDetailViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IEventAggregator eventAggregator) 
      : base(navigationService, pageDialogService)
    {
      _eventAggregator = eventAggregator;
    }
  }
}
