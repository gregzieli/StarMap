using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;
using Urho.Shapes;
using Urho.Urho2D;

namespace UhroTest
{
  public class Stars : Application
  {
    [Preserve]
    public Stars(ApplicationOptions options) : base(options)
    {
    }
    private void Application_UnhandledException(object sender, Urho.UnhandledExceptionEventArgs e)
    {
    }
    protected override void Stop()
    {
      base.Stop();
      //Urho.Application.UnhandledException -= Application_UnhandledException;
    }

    public Star SelectedStar { get; set; }

    public Sprite2D SelectionSprite { get; set; }
    Scene _scene;
    Octree _octree;
    Node _plotNode;
    Node _cameraNode;
    Camera _camera;
    List<Star> _stars;
    float yaw;
    float pitch;
    List<InputStar> input = new List<InputStar>()
    {
      new InputStar(55, 22, 66, 2, "1"),
      new InputStar(44, 99, 22, 5, "2"),
      new InputStar(13, 22, 41, 11, "3"),
      new InputStar(22, 22, 22, 4, "4"),
      new InputStar(23, 22, 91, 1, "5"),
      new InputStar(76, 21, 33, 1, "6"),
      new InputStar(41, 4, 62, 1, "7"),
      new InputStar(11, 122, 42, 1, "8"),
      new InputStar(78, 1, 15, 1, "9"),
      new InputStar(111, 74, 11, 1, "10"),
    };


    protected override void Start()
    {
      base.Start();
      CreateScene();
      SetupViewport();
    }

    private void OnTouched(TouchEndEventArgs e)
    {
      Ray cameraRay = _camera.GetScreenRay((float)e.X / Graphics.Width, (float)e.Y / Graphics.Height);
      var results = _octree.RaycastSingle(cameraRay, RayQueryLevel.Triangle, 100000, DrawableFlags.Geometry);
      if (results != null)
      {
        var star = results.Value.Node?.GetComponent<Star>();
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
    }

    void CreateScene()
    {
      Input.TouchEnd += OnTouched;
      _scene = new Scene();
      _octree = _scene.CreateComponent<Octree>();
      _plotNode = _scene.CreateChild();

      _cameraNode = _scene.CreateChild();
      _camera = _cameraNode.CreateComponent<Camera>();
      _cameraNode.Position = new Vector3(10, 35, 50);

      Node lightNode = _cameraNode.CreateChild();
      var light = lightNode.CreateComponent<Light>();
      light.LightType = LightType.Point;
      light.Range = 10000;
      light.Brightness = 1.3f;

      _stars = new List<Star>();
      foreach (var item in input)
      {
        var starNode = _plotNode.CreateChild();
        starNode.Position = new Vector3((float)item.X, (float)item.Y, (float)item.Z);

        var star = new Star(item);
        starNode.AddComponent(star);
        _stars.Add(star);
      }

      //SelectionSprite = ResourceCache.GetSprite2D("Selection.png");
    }
    
    void SetupViewport()
    {
      var renderer = Renderer;
      renderer.SetViewport(0, new Viewport(Context, _scene, _camera, null));
    }

    // This one worked too
    //protected override void OnUpdate(float timeStep)
    //{
    //  if (Input.NumTouches >= 1)
    //  {
    //    var touch = Input.GetTouch(0);
    //    var q = new Quaternion(touch.Delta.Y, touch.Delta.X, 0);
    //    _cameraNode.Rotate(q, TransformSpace.Local);
    //  }
    //  base.OnUpdate(timeStep);
    //}

    protected override void OnUpdate(float timeStep)
    {
      if (Input.NumTouches > 0)
      {
        // move
        if (Input.NumTouches == 1)
        {
          const float touchSensitivity = 2;
          TouchState state = Input.GetTouch(0);
          yaw += touchSensitivity * _camera.Fov / Graphics.Height * state.Delta.X;
          pitch += touchSensitivity * _camera.Fov / Graphics.Height * state.Delta.Y;
          _cameraNode.Rotation = new Quaternion(pitch, yaw, 0);
        }
        // multitouch zoom
        else if (Input.NumTouches == 2)
        {
          TouchState state1 = Input.GetTouch(0);
          TouchState state2 = Input.GetTouch(1);

          var distance1 = Distance(state1.Position, state2.Position);
          var distance2 = Distance(state1.LastPosition, state2.LastPosition);

          _cameraNode.Translate(new Vector3(0, 0, (distance1 - distance2) / 300f));
        }
      }

      base.OnUpdate(timeStep);
    }
    /// <summary>
		/// Distance between two 2D points (should be moved to IntVector2).
		/// </summary>
		float Distance(IntVector2 v1, IntVector2 v2)
    {
      return (float)Math.Sqrt((v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y));
    }
  }
}
