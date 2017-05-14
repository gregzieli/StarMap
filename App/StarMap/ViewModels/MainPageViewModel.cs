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
using System.Collections.Generic;

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
      _eventAggregator = eventAggregator;

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

      _eventAggregator.GetEvent<ConstellationSelectedEvent>().Subscribe(HandleConstellationRequest);
    }

    void HandleConstellationRequest(Constellation constellation)
    {

      Debug.WriteLine($"Selected {constellation?.Name ?? "null"}");
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




    /*
     * OK, so the options are two:
     * 1. Don't worry about unsubscribing, just use the filter upon subscribing or do a check in the handler (remember to lock!)
     *    - Subscription occurs in the constructor, so no additional check is required
     *      - new instance is created upon direct prism navigation, so not when hardware back button is used, 
     *        navigation page back button (probably, not checked), or when being on the main page and tapping MainPage in the MasterView.
     *      -  References to the disposed subscribers remain until the publisher gets disposed (MasterDetailVM in this case, which is long)
     *         - if keepSubscriberReferenceAlive is true, referenes stay even after that.
     * 2. Subscribing upon OnNavigatEDTo (not INGTo, because hardwareback button doesn't trigger it), unscubscribing with OnNavigatingFrom
     *    - keeps the subscribers in check, but I think it's the opposite to what prism's EventAggregator was made for in such scenario.
     *    
     *    I'm going with option 1 for now. But I think since the difference in publiser's and subscriber's lifespan, option 2 is better in this case.
     *    Because now the references are kept untill MasterDetailVM gets disposed, which is almost NEVER.
     *    
     *    OPTION 2:
    protected override async Task Restore()
    {
      // Need to check if the event is already subscribed, otherwise I'm resubscribing
      await Call(() =>
      {
        // If there are many events, it would be nice to have a generic collection (dictionary extension), 
        // that would allow to store <TEventType, Action<TPayload>> where TEventType : PubSubEvent<TPayload>
        // and just iterate through that. Since it's only one, ore few more, there's no need. It's rather not event ment that way, 
        // since to be perfectly thorough I would need two such collections, for events with and without the payload.
        var conEvent = _eventAggregator.GetEvent<ConstellationSelectedEvent>();

        // When the VM gets newed up by the Prism navigation, the action that this Contains checks differs from the previous one,
        // so that the result is false. That's why in this scenario I unsubscribe manually.
        //
        // It's the desired behavior. If not specified otherwise (keepSubscriberReferenceAlive), 
        // references to the subscriber (this) from the event are weak, which means, they get GC'd, along with this instance (eventually).
        if (!conEvent.Contains(HandleConstellationRequest))
          conEvent.Subscribe(HandleConstellationRequest);
      });
    }

    protected override async Task CleanUp()
    {
      await Call(() =>
      {
        _eventAggregator.GetEvent<ConstellationSelectedEvent>().Unsubscribe(HandleConstellationRequest);
      });
    }
     *    
     */


  }
}
