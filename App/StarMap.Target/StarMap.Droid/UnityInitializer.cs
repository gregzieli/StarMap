using Microsoft.Practices.Unity;
using Prism.Unity;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Abstractions.Services;
using StarMap.Droid.Database;
using StarMap.Droid.Sensors;

namespace StarMap.Droid
{
  public class UnityInitializer : IPlatformInitializer
  {
    public UnityInitializer()
    { }

    // TODO: use this way and inject IEventAggregator to device rotation service
    public void RegisterTypes(IUnityContainer container)
    {
      container
        .RegisterType<IRepository, AndroidRepository>()
        .RegisterType<IDeviceRotation, DeviceRotationImplementation>();
    }
  }
}