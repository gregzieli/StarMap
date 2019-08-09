using StarMap.Cll.Exceptions;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Extensions;
using StarMap.Core.Utils;
using System;
using System.Collections.Generic;
using Urho;
using Urho.Actions;
using Urho.Shapes;
using static Xamarin.Forms.Color;
using XFColor = Xamarin.Forms.Color;

namespace StarMap.Urhosharp
{
    // TODO: if in the sky mode the multi-star system is in fact rendered as few spheres close to one another,
    //       add a functionality to display more than one star here.
    // Because of that 'cant add component not on the main thread', the quickest solution would be to use a different class, like 'MultiDetail',
    // and in the view decide which one to use.
    public class SingleStar : UrhoBase
    {
        public SingleStar(ApplicationOptions options) : base(options) { }

        Node _starNode;

        [Obsolete("In the future, maybe, after change: not by color, but some other criteria")]
        IDictionary<XFColor, IList<Material>> GetTextures()
        {
            return new Dictionary<XFColor, IList<Material>>()
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
            var scale = star.AbsoluteMagnitude.Normalize(-8, 10, 1.5, 0.5);
            // Sometimes FillScene's gets code executes afther this one :(
            _starNode?.SetScale((float)scale);

            var light = _lightNode?.GetComponent<Light>();
            if (light != null)
                light.Color = new Color((float)star.Color.R, (float)star.Color.G, (float)star.Color.B);
        }

        protected override void OnUpdate(float timeStep)
        {
            if (Input.NumTouches >= 1)
            {
                var touch = Input.GetTouch(0);
                _cameraNode?.RotateAround(_starNode.Position, new Quaternion(-touch.Delta.Y * 0.05f, -touch.Delta.X * 0.05f, 0), TransformSpace.World);
            }
            base.OnUpdate(timeStep);
        }

        protected override void FillScene()
        {
            _cameraNode.Position = new Vector3(0, 0, -2);
            var skyboxNode = _scene.CreateChild();

            var skybox = skyboxNode.CreateComponent<Skybox>();
            skybox.Model = CoreAssets.Models.Box;
            skybox.SetMaterial(Material.SkyboxFromImage($"Textures/space{Randomizer.RandomInt(1, 2)}.png"));

            _starNode = _scene.CreateChild();

            var star = _starNode.CreateComponent<Sphere>();
            star.SetMaterial(Material.FromImage("Textures/white-dwarf2.jpg"));

            _starNode.RunActions(new RepeatForever(new RotateBy(duration: 1f, deltaAngleX: 0, deltaAngleY: 2, deltaAngleZ: 0)));
        }

        protected override void HandleException(Exception ex)
          => PublishError(new StarDetailUrhoException(ex));
    }
}
