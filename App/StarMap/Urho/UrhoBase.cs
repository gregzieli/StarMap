using Microsoft.Practices.Unity;
using Prism.Events;
using StarMap.Cll.Events;
using System;
using System.Threading.Tasks;
using Urho;

namespace StarMap.Urho
{
  public abstract class UrhoBase : Application
  {
    [Preserve]
    public UrhoBase(ApplicationOptions options) : base(options)
    {
    }
    
    // Urho Application is a special case: I cannot use constructor dependency injection
    // It breaks if the constructor looks any different than urho expects, although 
    // I leave base(options) as it is.
    //
    // For now this works. I can try (if time) a property injection;
    private IEventAggregator _eventAggregator = new EventAggregator();
    
    protected Scene _scene;
    protected Node _lightNode, _cameraNode;

    protected async Task RhunAsync(Func<Task> fn)
    {
      try
      {
        await fn?.Invoke();
      }
      catch (Exception e)
      {
        //HandleException(e);
      }      
    }

    protected abstract void HandleException(Exception ex);

    protected UrhoErrorEvent<T> GetErrorEvent<T>() where T : Exception
      => _eventAggregator.GetEvent<UrhoErrorEvent<T>>();

    protected void PublishError<T>(T payload) where T : Exception
    {
      Xamarin.Forms.MessagingCenter.Send(payload, "");
      //var e = GetErrorEvent<T>();
      
      //e.Publish(payload);
    }
  }
}
