using Microsoft.Practices.Unity;
using StarMap.Bll.Helpers;
using StarMap.Bll.Managers;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Abstractions.Services;
using StarMap.Dal.Providers;

namespace StarMap
{
  public class UnityBootstrapper
  {
    public static void RegisterTypes(IUnityContainer container)
    {
      // Nice fluent syntax
      container
        .RegisterType<ILocationManager, LocationManager>()
        .RegisterType<IAstronomer, Astronomer>()
        .RegisterType<IStarManager, StarManager>()
        .RegisterType<IStarDataProvider, StarDatabaseProvider>(
        // Thanks to Prism, the  xamarin DependencyService registrations are done automatically
        // https://github.com/PrismLibrary/Prism/blob/master/docs/Xamarin-Forms/5-Dependency-Service.md
        // : Prism simplifies this feature by allowing you to simply request any dependencies 
        //   that have been registered with Xamarin's DependencyService via your class constructor.
        /*new InjectionConstructor(DependencyService.Get<IDatabaseConnection>())*/);
    }
  }
}
