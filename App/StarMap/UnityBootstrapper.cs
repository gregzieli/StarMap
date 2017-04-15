using Microsoft.Practices.Unity;
using StarMap.Bll.Managers;
using StarMap.Cll.Abstractions;
using StarMap.Dal.Providers;
using Xamarin.Forms;

namespace StarMap
{
  public class UnityBootstrapper
  {
    public static void RegisterTypes(IUnityContainer container)
    {
      container
        .RegisterType<IStarManager, StarManager>()
        .RegisterType<IStarDataProvider, StarDatabaseProvider>(new InjectionConstructor(DependencyService.Get<IDatabaseConnection>()));
    }
  }
}
