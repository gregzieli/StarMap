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
using System;

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
    Node _lightNode, _cameraNode, _starNode;

    [Obsolete]
    public IDictionary<XFColor, IList<Material>> StarTextures2 { get; set; }
    public IList<Material> StarTextures { get; set; }

    private StarDetail _star;
    public StarDetail Star
    {
      get { return _star; }
      set { SetStar(value); }
    }

    protected override async void Start()
    {
      try
      {
        base.Start();
        GetTextures();
        await CreateScene();
      }
      catch (System.Exception e)
      {
        // TODO: perfect place for prism's pubsub event
        throw;
      }
    }

    void GetTextures()
    {
      // TODO: Not final
      StarTextures = new List<Material>
      {
        Material.FromImage("Textures/star_white1.jpg")
      };
    }

    [Obsolete]
    void GetTextures2()
    {
      // TODO: Not final
      StarTextures2 = new Dictionary<XFColor, IList<Material>>()
      {
        {
          DarkRed, new List<Material>()
            {
              Material.FromImage("Textures/star_red_dark1.jpg"),
              Material.FromImage("Textures/star_red_dark2.jpg"),
            }
        },
        {
          Red, new List<Material>()
            {
              Material.FromImage("Textures/star_red_light1.jpg"),
              Material.FromImage("Textures/star_red_light2.png"),
              Material.FromImage("Textures/star_red_light3.png")
            }
        },
        {
          Orange, new List<Material>
            {
              Material.FromImage("Textures/star_orange_dark1.jpg"),
              Material.FromImage("Textures/star_orange_dark2.png"),
              Material.FromImage("Textures/star_orange_light1.jpg"),
              Material.FromImage("Textures/star_orange_light2.png")
            }
        },
        {
          Yellow, new List<Material>
            {
              Material.FromImage("Textures/star_yellow_dark.png"),
              Material.FromImage("Textures/star_yellow_light.png")
            }
        },
        {
          White, new List<Material>
            {
              Material.FromImage("Textures/star_white1.jpg")
            }
        },
        {
          Blue, new List<Material>
            {
              Material.FromImage("Textures/star_lightblue_light.png"),
              Material.FromImage("Textures/star_lightblue_dark.png")
            }
        },
        {
          DarkBlue, new List<Material>
            {
              Material.FromImage("Textures/star_darkblue_dark.png"),
              Material.FromImage("Textures/star_darkblue_light.png")
            }
        }
      };
    }

    async Task CreateScene()
    {
      _scene = new Scene();
      _cameraNode = _scene.CreateChild();
      _starNode = _scene.CreateChild();
      _scene.CreateComponent<Octree>();

      Camera camera = _cameraNode.CreateComponent<Camera>();

      _lightNode = _cameraNode.CreateChild();
      Light light = _lightNode.CreateComponent<Light>();

      Node skyboxNode = _scene.CreateChild();
      skyboxNode.SetScale(100);
      Skybox skybox = skyboxNode.CreateComponent<Skybox>();
      skybox.Model = CoreAssets.Models.Box;
      skybox.SetMaterial(Material.SkyboxFromImage($"Textures/space{Randomizer.RandomInt(1, 2)}.png"));

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

      var scale = Normalizer.Normalize(star.AbsoluteMagnitude, -8, 10, 1.25, 0.5);
      _starNode.SetScale(Convert.ToSingle(scale));

      var a = _starNode.GetComponent<Sphere>();
      a.SetMaterial(StarTextures[Randomizer.RandomInt(0, StarTextures.Count - 1)]);

      var light = _lightNode.GetComponent<Light>();
      light.Color = new Color((float)star.Color.R, (float)star.Color.G, (float)star.Color.B);
    }
  }
}
