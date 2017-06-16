using StarMap.Cll.Exceptions;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Urho;
using Urho.Actions;
using Urho.Shapes;
using static Xamarin.Forms.Color;
using XFColor = Xamarin.Forms.Color;

namespace StarMap.Urho
{
  public class Universe : UrhoBase
  {
    public Universe(ApplicationOptions options) : base(options) { }

    protected override async Task FillScene()
    {
      //throw new NotImplementedException();
    }

    protected override void OnUpdate(float timeStep)
    {      
      base.OnUpdate(timeStep);
    }

    protected override void HandleException(Exception ex)
     => PublishError(new UniverseUrhoException(ex));
  }
}
