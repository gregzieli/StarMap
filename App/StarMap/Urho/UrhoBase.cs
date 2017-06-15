using System;
using System.Threading.Tasks;
using Urho;
using XF = Xamarin.Forms;

namespace StarMap.Urho
{
  public abstract class UrhoBase : Application
  {
    [Preserve]
    public UrhoBase(ApplicationOptions options) : base(options)
    { }
        
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
        // I thought it would be a perfect place for Prism's PubSubEvent. Unfortunately,
        // Urho Application is a special case: I cannot use constructor dependency injection
        // It breaks if the constructor looks any different than urho expects, although 
        // I leave base(options) as it is.
        //
        // That's why I'm using Xamarin's solution.
        HandleException(e);
      }      
    }

    protected abstract void HandleException(Exception ex);
    
    protected void PublishError<T>(T payload) where T : Exception
      // Must be main thread, else Android gets an exception "Can't create handler inside thread that has not called Looper.prepare()"
      => XF.Device.BeginInvokeOnMainThread(() => XF.MessagingCenter.Send(payload, string.Empty));
  }
}
