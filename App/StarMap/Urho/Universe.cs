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
using System.Collections.ObjectModel;
using StarMap.Urho.Components;

namespace StarMap.Urho
{
  public class Universe : UrhoBase
  {
    public Universe(ApplicationOptions options) : base(options) { }

    public StarComponent SelectedStar { get; set; }

    Node _plotNode;
    Camera _camera;
    const float touchSensitivity = 2;
    float _yaw, _pitch;

    protected override async Task FillScene()
    {
      //Input.TouchEnd += OnTouched;
      _camera = _cameraNode.GetComponent<Camera>();

      _plotNode = _scene.CreateChild();

      var light = _lightNode.GetComponent<Light>();
      light.LightType = LightType.Point;
      light.Range = 10000;
      light.Brightness = 1.3f;
    }

    public Star OnTouched(TouchEndEventArgs e)
    {
      Ray cameraRay = _camera.GetScreenRay((float)e.X / Graphics.Width, (float)e.Y / Graphics.Height);
      var results = _octree.RaycastSingle(cameraRay, RayQueryLevel.Triangle, 100000, DrawableFlags.Geometry);
      if (results != null)
      {
        var star = results.Value.Node?.GetComponent<StarComponent>();
        if (SelectedStar != star)
        {
          SelectedStar?.Deselect();
          SelectedStar = star;
          SelectedStar?.Select();
        }
      }
      else
      {
        SelectedStar?.Deselect();
        SelectedStar = null;
      }

      return SelectedStar?.StarData;
    }

    protected override void OnUpdate(float timeStep)
    {
      if (Input.NumTouches == 1)
      {
        TouchState state = Input.GetTouch(0);
        _yaw += touchSensitivity * _camera.Fov / Graphics.Height * state.Delta.X;
        _pitch += touchSensitivity * _camera.Fov / Graphics.Height * state.Delta.Y;
        _cameraNode.Rotation = new Quaternion(_pitch, _yaw, 0);
      }

      base.OnUpdate(timeStep);
    }

    protected override void HandleException(Exception ex)
     => PublishError(new UniverseUrhoException(ex));

    public void AddStars(IList<Star> stars)
    {
      foreach (var star in stars)
      {
        _plotNode.CreateChild().AddComponent(new StarComponent(star));
      }
    }
  }
}
