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
using Urho.Gui;
using Microsoft.Practices.ObjectBuilder2;

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
    const float VELOCITY = 3;//[pc/s]
    Vector3 _earthPosition = new Vector3(0, 0, 0);
    float _yaw, _pitch;

    protected override async Task FillScene()
    {
      _cameraNode.Position = _earthPosition;
      _camera = _cameraNode.GetComponent<Camera>();

      _plotNode = _scene.CreateChild();

      var light = _lightNode.GetComponent<Light>();
      //light.LightType = LightType.Directional;
      light.Range = 1000;
      //light.Brightness = 2;

      StarSprite = ResourceCache.GetSprite2D("Sprites/star.png");

      HighlightedStars = new List<StarComponent>();
    }


    public string OnTouched(TouchEndEventArgs e)
    {
      Ray cameraRay = _camera.GetScreenRay((float)e.X / Graphics.Width, (float)e.Y / Graphics.Height);
      var results = _octree.RaycastSingle(cameraRay, RayQueryLevel.Triangle, 10000, DrawableFlags.Geometry); 

      if (results != null)
      {
        // OK, so it doesn't work perfectly.
        // 1. if one node is closer than the other, but they are behind one another, i can never tap the one in the back.
        // 2. when travelling, some *collision* nodes are missing, dunno why.
        StarComponent star = results.Value.Drawable is Sphere sphere
          ? sphere.Node.Parent.GetComponent<StarComponent>()
          : results.Value.Drawable as StarComponent;

        if (star != null && SelectedStar != star)
        {
          SelectedStar?.Deselect();
          SelectedStar = star;
          SelectedStar.Select();
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

        var a = starNode.CreateChild("collision");
        var b = a.CreateComponent<Sphere>();
        a.ScaleNode(1.1f);
        // Seriously? No better way?
        b.Color = Color.Transparent;

        #region Physics (doesn't work)
        /*
        var body = starNode.CreateComponent<RigidBody>();
        body.SetCollisionLayerAndMask(2, 2);
        var collisionShape = starNode.CreateComponent<CollisionShape>();
        collisionShape.SetSphere(1, starNode.Position, Quaternion.Identity);
        */
        #endregion
      }

      MarkSun();

      foreach (var leftUnused in existingNodesById.Values)
      {
        leftUnused.RemoveComponent<StarComponent>();
        leftUnused.RemoveAllChildren();
        leftUnused.Remove();
      }
      existingNodesById.Clear();
    }

    public void HighlightStars(IEnumerable<IUnique> selectedStars)
    {
      ResetHighlight();
      foreach (var s in selectedStars)
      {
        var starComponent = _plotNode.GetChild(s.Id.ToString())?.GetComponent<StarComponent>();
        if (starComponent != null)
        {
          HighlightedStars.Add(starComponent);
          starComponent.Highlight();
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
      }

      HighlightedStars.Clear();
    }

    public void ToggleConstellations(IEnumerable<IUnique> selectedStars, bool turnOn)
    {
      foreach (var s in selectedStars)
      {
        var a = _plotNode.GetChild(s.Id.ToString());
        if (a != null)
        {
          a.Enabled = turnOn;
          a.GetChild("collision").Enabled = turnOn;
        }
      }
    }

    public Task Travel(IUnique star)
    {
      var target = _plotNode.GetChild(star.Id.ToString());
      
      var duration = Vector3.Distance(_cameraNode.Position, target.Position) / VELOCITY;
      
      // Then u can travel non-stop changing the destination
      _cameraNode.RemoveAllActions();

      Task travelTask = _cameraNode.RunActionsAsync(new MoveTo(duration, target.Position));

      _plotNode.GetChildrenWithComponent<StarComponent>().ForEach(x => x.LookAt(target.Position, Vector3.Up));      

      // If async task is the last thing to do, why bother awaiting it in here as well?
      return Task.WhenAll(travelTask, MarkSun(true));
    }

    public Task GoHome()
    {
      // Then u can travel non-stop changing the destination
      _cameraNode.RemoveAllActions();

      Task task = _cameraNode.RunActionsAsync(new MoveTo(1, _earthPosition));

      _plotNode.GetChildrenWithComponent<StarComponent>().ForEach(x => x.LookAt(_earthPosition, Vector3.Up));

      return Task.WhenAll(task, MarkSun(false));
    }

    void MarkSun()
    {
      var sunNode = _plotNode.GetChild("0");
      if (sunNode.GetChild("sol") != null)
        return;
      var a = sunNode.CreateChild("sol");
      a.SetScale(0.4f);
      var one = a.CreateChild();
      var two = a.CreateChild();

      one.CreateComponent<Torus>().Color = Color.Red;
      two.CreateComponent<Torus>().Color = Color.Red;

      one.RunActions(new RepeatForever(new RotateBy(1, 0, 0, 120)));
      two.RunActions(new RepeatForever(new RotateBy(1, 120, 0, 0)));

      a.SetDeepEnabled(false);
    }
    Task MarkSun(bool enable) => InvokeOnMainAsync(() => _plotNode.GetChild("0").GetChild("sol").SetDeepEnabled(enable));

    [Obsolete]
    void MarkSun1()
    {
      var sunNode = _plotNode.GetChild("0");
      var textNode = sunNode.CreateChild("text");
      // local position for a child is vector3.zero
      textNode.Position = new Vector3(0, 0.4f, 0);
      textNode.SetScale(4);
      var bb = textNode.CreateComponent<Text3D>();
      bb.SetFont(CoreAssets.Fonts.AnonymousPro, 10);
      bb.TextEffect = TextEffect.Stroke;
      bb.Text = "Sun";
    }
  }
}
