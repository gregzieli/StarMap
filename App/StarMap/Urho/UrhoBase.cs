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
    public UrhoBase(ApplicationOptions options, IEventAggregator ea) : base(options)
    {
      _eventAggregator = ea;
    }

    IEventAggregator _eventAggregator;
    protected Scene _scene;
    protected Node _lightNode, _cameraNode;

    protected async Task RhunAsync(Func<Task> fn)
    {
      try
      {
        await fn();
      }
      catch (Exception e)
      {
        HandleException(e);
      }      
    }

    protected abstract void HandleException(Exception ex);

    protected UrhoErrorEvent<T> GetErrorEvent<T>() where T : Exception
      => _eventAggregator.GetEvent<UrhoErrorEvent<T>>();

    protected void PublishError<T>(T payload) where T : Exception
      => GetErrorEvent<T>().Publish(payload);
  }
}
