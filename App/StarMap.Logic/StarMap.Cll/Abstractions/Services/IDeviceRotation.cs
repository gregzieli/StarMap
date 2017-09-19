using StarMap.Cll.Events;
using System;

namespace StarMap.Cll.Abstractions.Services
{
  public interface IDeviceRotation : IDisposable
  {
    void Start();

    void Stop();

    event EventHandler<RotationChangedEventArgs> RotationChanged;
  }
}
