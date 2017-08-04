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
  // TODO: if in the sky mode the multi-star system is in fact rendered as few spheres close to one another,
  //       add a functionality to display more than one star here.
  // Because of that 'cant add component not on the main thread', the quickest solution would be to use a different class, like 'MultiDetail',
  // and in the view decide which one to use.
  public class SingleStar : UrhoBase
  {
    public SingleStar(ApplicationOptions options) : base(options) { }

    Node _starNode;

    public IList<Material> StarTextures { get; set; }
    
    public StarDetail Star { get; set; }

    void GetTextures()
    {
      StarTextures = new List<Material>
      {
        Material.FromImage("Textures/white-dwarf2.jpg")//star_white1.jpg
      };
    }

    [Obsolete]
    public IDictionary<XFColor, IList<Material>> StarTextures2 { get; set; }
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


    public void SetStar(StarDetail star)
    {
      Star = star;

      var scale = Normalizer.Normalize(star.AbsoluteMagnitude, -8, 10, 1.5, 0.5);
      _starNode?.SetScale(Convert.ToSingle(scale));

      var a = _starNode?.GetComponent<Sphere>();
      a?.SetMaterial(StarTextures?[Randomizer.RandomInt(0, StarTextures.Count - 1)]);

      var light = _lightNode?.GetComponent<Light>();
      if (light != null)
        light.Color =  new Color((float)star.Color.R, (float)star.Color.G, (float)star.Color.B);
    }

    protected override void OnUpdate(float timeStep)
    {
      if (Input.NumTouches >= 1)
      {
        var touch = Input.GetTouch(0);
        _cameraNode?.RotateAround(_starNode.Position, new Quaternion(-touch.Delta.Y, -touch.Delta.X, 0), TransformSpace.World);
      }
      base.OnUpdate(timeStep);
    }

    protected override void FillScene()
    {
      GetTextures();

      _starNode = _scene.CreateChild();

      Node skyboxNode = _scene.CreateChild();
      skyboxNode.SetScale(100);
      Skybox skybox = skyboxNode.CreateComponent<Skybox>();
      skybox.Model = CoreAssets.Models.Box;
      skybox.SetMaterial(Material.SkyboxFromImage($"Textures/space{Randomizer.RandomInt(1, 2)}.png"));

      _starNode.Position = new Vector3(0, 0, 2);
      Sphere star = _starNode.CreateComponent<Sphere>();

      _starNode.RunActions(new RepeatForever(new RotateBy(duration: 1f, deltaAngleX: 0, deltaAngleY: 2, deltaAngleZ: 0)));
    }

    protected override void HandleException(Exception ex)
      => PublishError(new StarDetailUrhoException(ex));
  }
}
