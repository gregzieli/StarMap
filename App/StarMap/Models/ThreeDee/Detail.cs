using StarMap.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;
using Urho.Actions;
using Urho.Shapes;

namespace StarMap.Models.ThreeDee
{
  public class Detail : Application
  {
    [Preserve]
    public Detail(ApplicationOptions options) : base(options) { }

    Scene _scene;
    Octree _octree;
    // One parent node in case I want double+ systems.
    Node _plotNode;
    Node _cameraNode;
    Node _lightNode;
    Node _skyboxNode;
    Camera _camera;


    protected override void Start()
    {
      base.Start();
      CreateScene();
      SetupViewport();
    }

    void CreateScene()
    {
      _scene = new Scene();
      _octree = _scene.CreateComponent<Octree>();
      _plotNode = _scene.CreateChild();

      _cameraNode = _scene.CreateChild();
      _camera = _cameraNode.CreateComponent<Camera>();

      _lightNode = _cameraNode.CreateChild();
      _lightNode.CreateComponent<Light>();

      var starNode = _plotNode.CreateChild();
      starNode.Position = new Vector3(0, 0, 2);

      var star = starNode.CreateComponent<Box>();
      // add a texture
      star.SetMaterial(Material.FromImage($"Textures/star{Randomizer.RandomInt(1, 4)}.jpg"));
      // TODO: supply the color
      //star.Color = Color.Yellow;

      _skyboxNode = _scene.CreateChild();
      _skyboxNode.SetScale(100);
      var skybox = _skyboxNode.CreateComponent<Skybox>();
      skybox.Model = CoreAssets.Models.Box;
      skybox.SetMaterial(Material.SkyboxFromImage($"Textures/space{Randomizer.RandomInt(1, 3)}.png"));

      starNode.RunActions(new RepeatForever(new RotateBy(duration: 1f, deltaAngleX: 0, deltaAngleY: 1, deltaAngleZ: 0)));
    }

    void SetupViewport()
    {
      var renderer = Renderer;
      renderer.SetViewport(0, new Viewport(Context, _scene, _camera, null));
    }

    protected override void OnUpdate(float timeStep)
    {
      if (Input.NumTouches >= 1)
      {
        var touch = Input.GetTouch(0);
        _skyboxNode.Rotate(new Quaternion(-touch.Delta.Y, -touch.Delta.X, 0));
      }
      base.OnUpdate(timeStep);
    }
  }
}
