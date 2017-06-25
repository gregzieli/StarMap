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
using System.Linq;
using Urho.Urho2D;
using Urho.Physics;
using System.Diagnostics;
using StarMap.Core.Abstractions;
using StarMap.Core.Extensions;

namespace StarMap.Urho
{
  public class Universe : UrhoBase
  {
    public Universe(ApplicationOptions options) : base(options) { }

    public StarComponent SelectedStar { get; set; }

    public IList<StarComponent> HighlightedStars { get; set; }

    //https://github.com/xamarin/urho-samples/blob/master/FeatureSamples/Core/24_Urho2DSprite/Urho2DSprite.cs
    public Sprite2D StarSprite { get; set; }

    Node _plotNode;
    Camera _camera;
    const float touchSensitivity = 2;
    float _yaw, _pitch;

    protected override async Task FillScene()
    {
      _camera = _cameraNode.GetComponent<Camera>();

      _plotNode = _scene.CreateChild();

      var light = _lightNode.GetComponent<Light>();
      light.LightType = LightType.Point;
      light.Range = 10000;
      light.Brightness = 1.3f;

      StarSprite = ResourceCache.GetSprite2D("Sprites/star.png");

      HighlightedStars = new List<StarComponent>();
    }


    public string OnTouched(TouchEndEventArgs e)
    {
      Ray cameraRay = _camera.GetScreenRay((float)e.X / Graphics.Width, (float)e.Y / Graphics.Height);
      var results = _octree.RaycastSingle(cameraRay, RayQueryLevel.Aabb, 10000, DrawableFlags.Geometry); 

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

      return SelectedStar?.Node.Name;
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

    public void UpdateWithStars(IList<Star> stars)
    {
      var existingNodesById = _plotNode.GetChildrenWithComponent<StarComponent>()
        .ToDictionary(x => x.Name, x => x);

      foreach (var star in stars)
      {
        string id = star.Id.ToString();

        if (existingNodesById.ContainsKey(id))
        {
          existingNodesById.Remove(id);
          continue;
        }

        Node starNode = _plotNode.CreateChild(id, CreateMode.Local);
        var starComponent = starNode.CreateComponent<StarComponent>();

        starComponent.Sprite = StarSprite;

        starNode.Position = new Vector3(star.X, star.Y, star.Z);
        starNode.LookAt(_cameraNode.Position, Vector3.Up);

        #region Physics (doesn't work)
        /*
        var body = starNode.CreateComponent<RigidBody>();
        body.SetCollisionLayerAndMask(2, 2);
        var collisionShape = starNode.CreateComponent<CollisionShape>();
        collisionShape.SetSphere(1, starNode.Position, Quaternion.Identity);
        */
        #endregion

      }

      foreach (var leftUnused in existingNodesById.Values)
      {
        leftUnused.RemoveComponent<StarComponent>();
        leftUnused.Remove();
      }
      existingNodesById.Clear();
    }

    public void HighlightStars(IEnumerable<IUnique> selectedStars)
    {
      ResetHighlight();

      var aa = _plotNode.GetChildrenWithComponent<StarComponent>().OrderBy(x => x.Name);

      foreach (var s in selectedStars)
      {
        var starComponent = _plotNode.GetChild(s.Id.ToString())?.GetComponent<StarComponent>();
        if (starComponent != null)
        {
          HighlightedStars.Add(starComponent);
          starComponent.Color = Color.Cyan;
          starComponent.Select2();
        }
      }
    }

    public void ResetHighlight()
    {
      if (HighlightedStars.IsNullOrEmpty())
        return;

      foreach (var s in HighlightedStars)
      {
        s.Deselect2();
        s.Color = Color.White;
      }

      HighlightedStars.Clear();
    }
  }
}
