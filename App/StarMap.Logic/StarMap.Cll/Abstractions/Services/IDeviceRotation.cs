namespace StarMap.Cll.Abstractions.Services
{
  public interface IDeviceRotation
  {
    Rotation GetDeviceRotation();
  }

  // Move this class to Core
  public class Rotation
  {
    public float[] Values { get; set; }

    public float Azimuth => Values[0];

    public float Pitch => Values[1];

    public float Roll => Values[2];
  }
}
