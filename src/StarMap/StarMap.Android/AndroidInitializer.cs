using Prism;
using Prism.Ioc;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Abstractions.Services;
using StarMap.Droid.Database;
using StarMap.Droid.Sensors;

namespace StarMap.Droid
{
    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            // TODO: use this way and inject IEventAggregator to device rotation service
            containerRegistry.Register<IRepository, AndroidRepository>();
            containerRegistry.Register<IDeviceRotation, DeviceRotationImplementation>();
        }
    }
}