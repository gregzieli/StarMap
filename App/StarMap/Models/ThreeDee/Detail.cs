using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Utils;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Urho;
using Urho.Actions;
using Urho.Shapes;
using XFColor = Xamarin.Forms.Color;
using static Xamarin.Forms.Color;

namespace StarMap.Models.ThreeDee
{
  public class Detail : Application
  {
    // TODO: if in the sky mode the multi-star system is in fact rendered as few spheres close to one another,
    //       add a functionality to display more than one star here.
    // Because of that 'cant add component not on the main thread', the quickest solution would be to use a different class, like 'MultiDetail',
    // and in the view decide which one to use.
    [Preserve]
    public Detail(ApplicationOptions options) : base(options) { }

    Scene _scene;
    Node _cameraNode;
    Node _starNode;

    public IDictionary<XFColor, Material> StarTextures { get; set; }

    private StarDetail _star;
    public StarDetail Star
    {
      get { return _star; }
      set { SetStar(value); }
    }


    protected override async void Start()
    {
      base.Start();
      GetTextures();
      await CreateScene();
    }

    void GetTextures()
    {
      // TODO
      StarTextures = new Dictionary<XFColor, Material>()
      {
        { Red, Material.FromImage("Textures/star1.jpg") },
        { Orange, Material.FromImage("Textures/star2.jpg") },
        { Yellow, Material.FromImage("Textures/star3.jpg") },
        { White, Material.FromImage("Textures/star2.jpg") },
        { Blue, Material.FromImage("Textures/star3.jpg") }
      };
    }

    async Task CreateScene()
    {
      _scene = new Scene();
      _cameraNode = _scene.CreateChild();
      _starNode = _scene.CreateChild();
      _scene.CreateComponent<Octree>();

      Camera camera = _cameraNode.CreateComponent<Camera>();

      Node lightNode = _cameraNode.CreateChild();
      Light light = lightNode.CreateComponent<Light>();

      Node skyboxNode = _scene.CreateChild();
      skyboxNode.SetScale(100);
      Skybox skybox = skyboxNode.CreateComponent<Skybox>();
      skybox.Model = CoreAssets.Models.Box;
      skybox.SetMaterial(Material.SkyboxFromImage($"Textures/space{Randomizer.RandomInt(1, 3)}.png"));

      _starNode.Position = new Vector3(0, 0, 2);
      Sphere star = _starNode.CreateComponent<Sphere>();

      Renderer.SetViewport(0, new Viewport(Context, _scene, camera, null));

      await _starNode.RunActionsAsync(new RepeatForever(new RotateBy(duration: 1f, deltaAngleX: 0, deltaAngleY: 1, deltaAngleZ: 0)));
    }

    protected override void OnUpdate(float timeStep)
    {
      if (Input.NumTouches >= 1)
      {
        var touch = Input.GetTouch(0);
        _cameraNode.RotateAround(_starNode.Position, new Quaternion(-touch.Delta.Y, -touch.Delta.X, 0), TransformSpace.World);
      }
      base.OnUpdate(timeStep);
    }
    
    void SetStar(StarDetail star)
    {
      _star = star;
      var a = _starNode.GetComponent<Sphere>();
      a.SetMaterial(StarTextures[star.Color]);
    }
  }
}
