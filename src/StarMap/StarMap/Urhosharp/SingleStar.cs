using StarMap.Cll.Exceptions;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Extensions;
using StarMap.Core.Utils;
using System;
using Urho;
using Urho.Actions;
using Urho.Shapes;

namespace StarMap.Urhosharp
{
    public class SingleStar : UrhoBase
    {
        public SingleStar(ApplicationOptions options) : base(options) { }

        private Node _starNode;

        public void SetStar(StarDetail star)
        {
            var scale = star.AbsoluteMagnitude.Normalize(-8, 10, 1.5, 0.5);
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
