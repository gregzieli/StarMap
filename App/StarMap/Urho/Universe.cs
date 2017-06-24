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

namespace StarMap.Urho
{
  public class Universe : UrhoBase
  {
    public Universe(ApplicationOptions options) : base(options) { }

    public StarComponent SelectedStar { get; set; }

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

    public void AddStars2(IList<Star> stars)
    {
      foreach (var star in stars)
      {
        _plotNode.CreateChild().AddComponent(new StarComponent(star));
      }
    }

    public void AddStars(IList<Star> stars)
    {
      foreach (var star in stars)
      {
        var starNode = _plotNode.CreateChild();
        var a = starNode.CreateComponent<StaticSprite2D>();
        
        a.Sprite = StarSprite;

        starNode.Position = new Vector3(star.X, star.Y, star.Z);
        starNode.LookAt(_cameraNode.Position, Vector3.Up);
      }
    }

    public void UpdateStarsLeaveNodes(IList<Star> stars)
    {
      var nodesById = _plotNode.GetChildrenWithComponent<StarComponent>()
        .ToDictionary(x => x.ID, x => x);

      var starsById = _plotNode.GetChildrenWithComponent<StarComponent>()
        .Select(x => x.GetComponent<StarComponent>())
        .ToDictionary(x => x.ID, x => x);

      var inputById = stars.ToDictionary(x => (uint)x.Id, x => x);

      var previousUrhoIdsToBeRemoved = nodesById.Keys.Except(inputById.Keys);

      foreach (var s in stars)
      {
        uint id = Convert.ToUInt32(s.Id);

        // if I keep Ids on both node and component level, it will be easier (faster) to find.
        var sef = _plotNode.GetChild(id);
        if (sef != null)
        {
          // It's already here: no update.
          ; 
        }
        else
        {
          //https://github.com/xamarin/urho-samples/blob/master/FeatureSamples/Core/24_Urho2DSprite/Urho2DSprite.cs
          var starNode = _plotNode.CreateChild(id, CreateMode.Local);
          
          var a = starNode.CreateComponent<StaticSprite2D>(CreateMode.Local, id);
          a.Color = Color.White;
          // Check documentation for what this is; probably not even needed
          a.BlendMode = BlendMode.Alpha;
          a.Sprite = StarSprite;
        }
      }
    }

    public void AddOrUpdateStar(Star star)
    {
      uint id = Convert.ToUInt32(star.Id);
      var starNode = _plotNode.GetChild(id) ?? _plotNode.CreateChild(id, CreateMode.Local);

      var starComponent = starNode.GetOrCreateComponent<StarComponent>();

    }
  }
}
