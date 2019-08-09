using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;

namespace StarMap.ViewModels.Core
{
    // Official prism documentation
    // https://github.com/PrismLibrary/Prism/blob/master/docs/WPF/09-Communication.md
    // says that 'The PubSubEvent<TPayload> is intended to be the base class for an application's or module's specific events'
    // So using this class would be against that. And to have generics using this custom event class would be an overkill, since 
    // the methods would need two type params (where TEventType : PubSubEvent<TPayload>).
    // OK, so really:
    // If it turns out I only use one TPayload per event, it's fine to use this class, otherwise custom events are required.
    public abstract class Publisher : Navigator
    {
        IEventAggregator _eventAggregator;
        public Publisher(INavigationService navigationService, IPageDialogService pageDialogService, IEventAggregator eventAggregator) : base(navigationService, pageDialogService)
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
            await Call(() =>
            {
                GetEvent<TPayload>().Publish(payload);
            });
        }
    }
}
