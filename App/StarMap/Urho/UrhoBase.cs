using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;

namespace StarMap.Urho
{
  public class UrhoBase : Application
  {
    [Preserve]
    public UrhoBase(ApplicationOptions options) : base(options) { }

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
        // TODO: perfect place for prism's pubsub event
        
      }
    }
  }
}
