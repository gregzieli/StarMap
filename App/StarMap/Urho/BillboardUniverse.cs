using StarMap.Cll.Exceptions;
using StarMap.Cll.Models.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;

namespace StarMap.Urho
{
  public class BillboardUniverse : UrhoBase
  {
    public BillboardUniverse(ApplicationOptions options) : base(options) {  }

    Node _plotNode;
    Camera _camera;
    const float touchSensitivity = 2;
    float _yaw, _pitch;

    IList<uint> _currentStarIds = new List<uint>();

    BillboardSet _billboardSet;

    protected override async Task FillScene()
    {
      _camera = _cameraNode.GetComponent<Camera>();

      _plotNode = _scene.CreateChild();
      _billboardSet = _plotNode.CreateComponent<BillboardSet>();
      //_billboardSet.FixedScreenSize = true;
      _billboardSet.Material = Material.FromImage("Sprites/star.png");
      _billboardSet.NumBillboards = 15000;

      var light = _lightNode.GetComponent<Light>();
      light.LightType = LightType.Point;
      light.Range = 10000;
      light.Brightness = 1.3f;
    }

    public void UpdateWithStars(IList<Star> stars)
    {
      var newIds = stars.Select(x => (uint)x.Id).ToList();
      var unusedIds = _currentStarIds.Except(newIds);
      _billboardSet.FixedScreenSize = true; // DOES NOT WORK!!! 

      foreach (var uid in unusedIds)
      {
        var b = _billboardSet.GetBillboardSafe(uid);
        b.Enabled = false;
      }
      uint id = 0;
      foreach (var star in stars)
      {
        var bil = _billboardSet.GetBillboardSafe(id);
        if (bil.Enabled)
          continue;
        bil.Color = Color.White;
        bil.Position = new Vector3(star.X, star.Y, star.Z);
        bil.Size = new Vector2(4f, 4f);
        bil.Enabled = true;
        id++;
      }

      _billboardSet.Commit();
    }

    public string OnTouched(TouchEndEventArgs e)
    {
      Ray cameraRay = _camera.GetScreenRay((float)e.X / Graphics.Width, (float)e.Y / Graphics.Height);
      var results = _octree.RaycastSingle(cameraRay, RayQueryLevel.Triangle, 10000, DrawableFlags.Geometry);

      if (results != null)
      {
      }
      return null;
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
  }
}
