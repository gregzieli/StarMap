using StarMap.Cll.Exceptions;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Abstractions;
using StarMap.Core.Extensions;
using StarMap.Urhosharp.UrhoComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Urho;
using Urho.Actions;
using Urho.Physics;
using Urho.Shapes;
using Urho.Urho2D;

namespace StarMap.Urhosharp
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

        private Node _plotNode;
        private Camera _camera;
        private const float touchSensitivity = 1f;
        private const float VELOCITY = 3; //[pc/s]

        private Matrix3 _rotationMatrix = new Matrix3();
        private bool _usingSensors = false;
        private PhysicsWorld _physics;
        private PhysicsRaycastResult _rayCast;

        protected override void FillScene()
        {
            _physics = _scene.CreateComponent<PhysicsWorld>();
            _rayCast = new PhysicsRaycastResult();

            _camera = _cameraNode.GetComponent<Camera>();
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
            SelectedStar?.Deselect();
            SelectedStar = null;
            _plotNode.RemoveAllChildren();

            string id;
            foreach (var star in stars)
            {
                id = star.Id.ToString();

                var starNode = _plotNode.CreateChild(id, CreateMode.Local);
                var starComponent = starNode.CreateComponent<StarComponent>();

                starComponent.Sprite = StarSprite;
                float scale = 1;

                // Scale by absolute magnitude
                if (star.AbsoluteMagnitude < 2)
                    scale = (float)star.AbsoluteMagnitude.Normalize(-14, 1, 4, 1.2);

                starNode.Scale = new Vector3(scale, scale, scale);

                starNode.Position = new Vector3(star.X, star.Y, star.Z);
                starNode.LookAt(_cameraNode.Position, Vector3.Up);
                starNode.AddCollisionSupport(0.1f);
            }

            MarkSun().SetDeepEnabled(!IsHome);
        }


        public string OnTouched(TouchEndEventArgs e, out float relativeDistance)
        {
            relativeDistance = default;
            var cameraRay = _camera.GetScreenRay(e.X.Normalize(Graphics.Width), e.Y.Normalize(Graphics.Height));

            _physics.SphereCast(ref _rayCast, cameraRay, 0.4f, 10000);
            if (_rayCast.Body != null)
            {
                // It's a bit annoying to get the component only to operate on it's node,
                // But at least I could put some logic into that class, and not here.
                // Because really, it could be 'selectedNode', and 'Select-Deselect' methods done here.
                var star = _rayCast.Body.Node.Parent.GetComponent<StarComponent>();
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

        public void SetRotation(Matrix3 rotation)
        {
            _usingSensors = true;
            _rotationMatrix = rotation;
        }

        protected override void OnUpdate(float timeStep)
        {
            if (_usingSensors)
                UpdateBySensor();
            else
                UpdateByTouch();

            base.OnUpdate(timeStep);
        }

        private void UpdateBySensor()
        {
            _cameraNode.Rotation = new Quaternion(ref _rotationMatrix);
        }

        private void UpdateByTouch()
        {
            if (Input.NumTouches == 1)
            {
                var state = Input.GetTouch(0);

                // From the WorkBooks (Exploring Urho Coordinates): 
                // The Pitch, Yaw, and Roll methods are named after terms used in aerodynamics 
                // and perform accumulative rotations around the X, Y, and Z axes, respectively.
                // 
                // Math taken from urho-samples SmartHome (https://github.com/xamarin/urho-samples/blob/master/SmartHome/Clients/SmartHouse/UrhoApp.cs)
                _cameraNode.Yaw(touchSensitivity * _camera.Fov / Graphics.Height * state.Delta.X);
                _cameraNode.Pitch(touchSensitivity * _camera.Fov / Graphics.Height * state.Delta.Y);
            }
        }

        protected override void HandleException(Exception ex)
         => PublishError(new UniverseUrhoException(ex));

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

            _cameraNode.RemoveAllActions();

            var travelTask = _cameraNode.TryRunActionsAsync(new MoveTo(duration, target.Position));

            foreach (var x in _plotNode.GetChildrenWithComponent<StarComponent>())
                x.LookAt(target.Position, Vector3.Up);

            await travelTask;

            CurrentLocation = star;
            await MarkSun(!IsHome);
        }

        private Node MarkSun()
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

        private Task MarkSun(bool enable)
          => InvokeOnMainAsync(() => _plotNode.GetChild("0").GetChild("sol").SetDeepEnabled(enable));
    }
}
