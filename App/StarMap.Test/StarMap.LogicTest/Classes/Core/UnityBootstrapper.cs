using Microsoft.Practices.Unity;
using StarMap.Bll.Managers;
using StarMap.Cll.Abstractions;
using StarMap.Dal.Providers;

namespace StarMap.LogicTest.Classes
{
  public static class UnityBootstrapper
  {
    public static void RegisterTypes(IUnityContainer container)
    {
      container
        .RegisterType<IStarDataProvider, StarDatabaseProvider>(new InjectionConstructor(new MockConnection()))
        .RegisterType<IStarManager, StarManager>()
        .RegisterType<ILocationManager, LocationManager>();
    }
  }
}
