using Microsoft.Practices.Unity;
using NUnit.Framework;
using StarMap.Cll.Abstractions;
using StarMap.Common.Test;
using System.Linq;

namespace StarMap.LogicTest
{
  [TestFixture]
  public class Database : SetupFixture
  {
    [Test]
    public void ShowUniverse()
    {
      var manager = Container.Resolve<IStarManager>();
      var a = manager.GetConstellations();

      Assert.IsTrue(a.Count() > 0);
    }

    [Test]
    public void StarDetailMapping()
    {
      var prov = Container.Resolve<IStarDataProvider>();
      var all = prov.GetStars(new Cll.Filters.StarFilter()
      {
        DistanceTo = 100000,
        MagnitudeTo = 100000
      });// It takes a few minutes for all

      var e = all.GetEnumerator();
      Assert.DoesNotThrow(new TestDelegate(() => 
      {
        while (e.MoveNext()) prov.GetStarDetails(e.Current.Id);
      }));      
    }
  }
}
