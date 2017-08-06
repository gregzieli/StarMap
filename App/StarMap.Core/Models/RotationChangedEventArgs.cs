using System;

namespace StarMap.Core.Models
{
  public class RotationChangedEventArgs : EventArgs
  {
    public RotationChangedEventArgs(float[] orientation)
    {
      Orientation = orientation;
    }

    public float[] Orientation { get; private set; }

    public float Azimuth => Orientation[0];

    public float Pitch => Orientation[1];

    public float Roll => Orientation[2];
  }
}
