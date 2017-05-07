using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using Prism.Events;

namespace StarMap.ViewModels.Core
{
  // Official prism documentation
  // https://github.com/PrismLibrary/Prism/blob/master/docs/WPF/09-Communication.md
  // says that 'The PubSubEvent<TPayload> is intended to be the base class for an application's or module's specific events'
  // So using this class would be against that. And to have generics using this custom event class would be an overkill, since 
  // the methods would need two type params (where TEventType : PubSubEvent<TPayload>).
  // If there's time, and after deeper reading, if it turns out there is no need to use the custom event class, use this one.
  public abstract class Herald : Navigator
  {
    IEventAggregator _eventAggregator;
    public Herald(INavigationService navigationService, IPageDialogService pageDialogService, IEventAggregator eventAggregator) : base(navigationService, pageDialogService)
    {
      _eventAggregator = eventAggregator;
    }

    /// <summary>
    /// Gets an instance of an event type.
    /// </summary>
    /// <typeparam name="TPayload">Type of the payload of the event.</typeparam>
    /// <returns>an event that can be subscribed to or published.</returns>
    protected PubSubEvent<TPayload> GetEvent<TPayload>() => _eventAggregator.GetEvent<PubSubEvent<TPayload>>();

    /// <summary>
    /// Publishes event with the specified payload, using Prism's PubSubEvent.
    /// </summary>
    /// <typeparam name="TPayload">Type of the payload of the event.</typeparam>
    /// <param name="payload">Message to pass to the subscribers.</param>
    protected async Task PublishEvent<TPayload>(TPayload payload)
    {
      // I think having a generic publisher is OK, but a subscriber MUST be handled individually,
     // so Prism can properly unsubscribe it.
      await Call(() =>
      {
        GetEvent<TPayload>().Publish(payload);
      });
    }
  }
}
