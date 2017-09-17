using Microsoft.Practices.ObjectBuilder2;
using StarMap.Cll.Exceptions;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Abstractions;
using StarMap.Core.Extensions;
using StarMap.Urho.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Urho;
using Urho.Actions;
using Urho.Physics;
using Urho.Shapes;
using Urho.Urho2D;

namespace StarMap.Urho
{
  public class Universe : UrhoBase
  {
    public Universe(ApplicationOptions options) : base(options) { }

    public StarComponent SelectedStar { get; set; }

    public IUnique CurrentLocation { get; set; }

    public bool IsHome => CurrentLocation?.Id == 0;

    public IList<StarComponent> HighlightedStars { get; set; }

    //https://github.com/xamarin/urho-samples/blob/master/FeatureSamples/Core/24_Urho2DSprite/Urho2DSprite.cs
    public Sprite2D StarSprite { get; set; }

    Node _plotNode;
    Camera _camera;
    const float touchSensitivity = 1f;
    const float VELOCITY = 3;//[pc/s]
    const float RAD2DEG = 180 / (float)Math.PI;
    float _yaw, _pitch, _roll;

    bool _usingSensors = false;
    PhysicsWorld _physics;
    PhysicsRaycastResult _rayCast;

    protected override void FillScene()
    {
      _physics = _scene.CreateComponent<PhysicsWorld>();
      _rayCast = new PhysicsRaycastResult();

      _camera = _cameraNode.GetComponent<Camera>();
      //// From Xamarin workbooks: Setting higher Field of View (default is 45[deg]) works as *zooming out*
      //// but e.g. 90 wierdly skews the view, and the Sun is still not visible.
      //_camera.Fov = 45;
      // Not sure if it changes anything. This is the smallest value possible.
      //_camera.NearClip = 0.010000599f;

      _plotNode = _scene.CreateChild();

      var light = _lightNode.GetComponent<Light>();
      light.Range = 1000;

      StarSprite = ResourceCache.GetSprite2D("Sprites/star.png");
      
      HighlightedStars = new List<StarComponent>();
    }

    public void UpdateWithStars(IList<Star> stars, IUnique currentPosition)
    {
      var currentStar = stars.FirstOrDefault(x => x.Id == currentPosition.Id);
      _cameraNode.Position = new Vector3(currentStar.X, currentStar.Y, currentStar.Z);
      CurrentLocation = currentStar;

      _plotNode.RemoveAllChildren();

      string id;
      foreach (var star in stars)
      {
        id = star.Id.ToString();

        Node starNode = _plotNode.CreateChild(id, CreateMode.Local);
        var starComponent = starNode.CreateComponent<StarComponent>();

        starComponent.Sprite = StarSprite;
        float scale = 1;

        // Scale by absolute magnitude
        if (star.AbsoluteMagnitude < 2)
          scale = (float)star.AbsoluteMagnitude.Normalize(-14, 1, 4, 1.2);

        starNode.Scale = new Vector3(scale, scale, scale);
        // haha this throws error sometimes when moving away from the page
        //starNode.SetScale(scale);

        starNode.Position = new Vector3(star.X, star.Y, star.Z);
        starNode.LookAt(_cameraNode.Position, Vector3.Up);
        starNode.AddCollisionSupport(0.1f);
      }

      MarkSun().SetDeepEnabled(!IsHome);
    }


    public string OnTouched(TouchEndEventArgs e, out float relativeDistance)
    {
      relativeDistance = default(float);
      Ray cameraRay = _camera.GetScreenRay(e.X.Normalize(Graphics.Width), e.Y.Normalize(Graphics.Height));

      _physics.SphereCast(ref _rayCast, cameraRay, 0.4f, 10000);
      if (_rayCast.Body != null)
      {
        // It's a bit annoying to get the component only to operate on it's node,
        // But at least I could put some logic into that class, and not here.
        // Because really, it could be 'selectedNode', and 'Select-Deselect' methods done here.
        StarComponent star = _rayCast.Body.Node.Parent.GetComponent<StarComponent>();
        if (SelectedStar != star)
        {
          SelectedStar?.Deselect();
          SelectedStar = star;
          SelectedStar.Select();
          relativeDistance = Vector3.Distance(_cameraNode.Position, star.Node.Position);
        }
      }
      else
      {
        SelectedStar?.Deselect();
        SelectedStar = null;
      }

      return SelectedStar?.Node.Name;
    }

    public void SetRotation(float[] orientation)
    {
      _usingSensors = true;
      _yaw = (int)Math.Round(orientation[0] * RAD2DEG);
      _pitch = (int)Math.Round(orientation[1] * RAD2DEG);
      //_roll = (int)Math.Round(orientation[2] * RAD2DEG);
    }
    
    protected override void OnUpdate(float timeStep)
    {
      if (_usingSensors)
        UpdateBySensor();
      else
        UpdateByTouch();

      base.OnUpdate(timeStep);
    }

    void UpdateBySensor()
    {
      _cameraNode.Rotation = new Quaternion(_pitch, _yaw, 0);
    }

    void UpdateByTouch()
    {
      if (Input.NumTouches == 1)
      {
        TouchState state = Input.GetTouch(0);

        // From the WorkBooks (Exploring Urho Coordinates): 
        // The Pitch, Yaw, and Roll methods are named after terms used in aerodynamics 
        // and perform accumulative rotations around the X, Y, and Z axes, respectively.
        _cameraNode.Yaw(touchSensitivity * _camera.Fov / Graphics.Height * state.Delta.X);
        _cameraNode.Pitch(touchSensitivity * _camera.Fov / Graphics.Width * state.Delta.Y);
      }
    }

    protected override void HandleException(Exception ex)
     => PublishError(new UniverseUrhoException(ex));

    [Obsolete("I thought this algorithm would be helpful, but as it turns out, it's much slower than simply *plotNode.RemoveAll*")]
    public void UpdateWithStars2(IList<Star> stars, IUnique currentPosition)
    {
      var currentStar = stars.FirstOrDefault(x => x.Id == currentPosition.Id);
      _cameraNode.Position = new Vector3(currentStar.X, currentStar.Y, currentStar.Z);
      CurrentLocation = currentStar;

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
        float scale = 1;

        // Scale by absolute magnitude
        if (star.AbsoluteMagnitude < 2)
          scale = (float)star.AbsoluteMagnitude.Normalize(-14, 1, 2, 1.2);

        starNode.Scale = new Vector3(scale, scale, scale);
        // haha this throws error sometimes when moving away from the page
        //starNode.SetScale(scale);

        starNode.Position = new Vector3(star.X, star.Y, star.Z);
        starNode.LookAt(_cameraNode.Position, Vector3.Up);
        starNode.AddCollisionSupport(0.1f);
      }

      MarkSun().SetDeepEnabled(!IsHome);
      
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

      foreach (var star in HighlightedStars)
        star.Dim();

      HighlightedStars.Clear();
    }

    public void ToggleConstellations(IEnumerable<IUnique> selectedStars, bool turnOn)
    {
      foreach (var s in selectedStars)
      {
        var node = _plotNode.GetChild(s.Id.ToString());
        if (node != null)
        {
          node.Enabled = turnOn;
          node.GetChild("collision").Enabled = turnOn;
        }
      }
    }

    public async Task Travel(IUnique star)
    {
      var target = _plotNode.GetChild(star.Id.ToString());
      
      var duration = Vector3.Distance(_cameraNode.Position, target.Position) / VELOCITY;
      
      // Then u can travel non-stop changing the destination
      _cameraNode.RemoveAllActions();

      Task travelTask = _cameraNode.RunActionsAsync(new MoveTo(duration, target.Position));
      
      _plotNode.GetChildrenWithComponent<StarComponent>()
        .ForEach(x => x.LookAt(target.Position, Vector3.Up));

      await travelTask;

      CurrentLocation = star;
      await MarkSun(!IsHome);
    }    

    Node MarkSun()
    {
      Node sol;
      var sunNode = _plotNode.GetChild("0");
      if ((sol = sunNode.GetChild("sol")) != null)
        return sol;

      sol = sunNode.CreateChild("sol");
      sol.SetScale(0.4f);
      var one = sol.CreateChild();
      var two = sol.CreateChild();

      one.CreateComponent<Torus>().Color = Color.Red;
      two.CreateComponent<Torus>().Color = Color.Red;

      one.RunActions(new RepeatForever(new RotateBy(1, 0, 0, 120)));
      two.RunActions(new RepeatForever(new RotateBy(1, 120, 0, 0)));

      return sol;
    }

    Task MarkSun(bool enable) 
      => InvokeOnMainAsync(() => _plotNode.GetChild("0").GetChild("sol").SetDeepEnabled(enable));
    
  }
}
