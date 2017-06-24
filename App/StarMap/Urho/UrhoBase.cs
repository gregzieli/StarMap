using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Urho;
using Urho.Gui;
using XF = Xamarin.Forms;

namespace StarMap.Urho
{
  public abstract class UrhoBase : Application
  {
    [Preserve]
    public UrhoBase(ApplicationOptions options) : base(options)
    { }

    // Check if this way still an exception doesn't get caught by the Call method from VM.
    // Then consider if it's not even a better solution to just swallow them.
    static UrhoBase()
    {
      UnhandledException += (s, e) =>
      {
#if DEBUG
        Debugger.Break();
#endif
        e.Handled = true;
      };
    }

    protected Scene _scene;
    protected Octree _octree;
    protected Node _lightNode, _cameraNode;

    protected abstract void HandleException(Exception ex);
    protected abstract Task FillScene();

    protected override async void Start()
    {
      try
      {
        base.Start();
        CreateScene();
        await FillScene().ConfigureAwait(false);
      }
      catch (Exception e)
      {
        HandleException(e);
        await Exit();
      }
    }

    void CreateScene()
    {
      _scene = new Scene();
      _octree = _scene.CreateComponent<Octree>();
      _cameraNode = _scene.CreateChild();
      _cameraNode.CreateComponent<Camera>();
      _lightNode = _cameraNode.CreateChild();
      _lightNode.CreateComponent<Light>();

      Renderer.SetViewport(0, new Viewport(Context, _scene, _cameraNode.GetComponent<Camera>(), null));
    }
    
    // I thought it would be a perfect place for Prism's PubSubEvent. Unfortunately,
    // Urho Application is a special case: I cannot use constructor dependency injection
    // It breaks if the constructor looks any different than urho expects, although 
    // I leave base(options) as it is.
    //
    // That's why I'm using Xamarin's solution.
    protected void PublishError<T>(T payload) where T : Exception
      // Must be main thread, else Android gets an exception "Can't create handler inside thread that has not called Looper.prepare()"
      => XF.Device.BeginInvokeOnMainThread(() => XF.MessagingCenter.Send(payload, string.Empty));

    protected async Task RhunAsync(Func<Task> fn)
    {
      try
      {
        await fn?.Invoke();
      }
      catch (Exception e)
      {
        HandleException(e);
        await Exit();
      }
    }
  }
}
