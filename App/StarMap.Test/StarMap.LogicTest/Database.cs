using Microsoft.Practices.Unity;
using NUnit.Framework;
using StarMap.Cll.Abstractions;
using StarMap.Common.Test;
using System.Linq;
using StarMap.Core.Utils;
using System.Threading.Tasks;

namespace StarMap.LogicTest
{
  [TestFixture]
  public class Database : SetupFixture
  {
    [Test]
    public void ShowUniverse()
    {
      var prov = Container.Resolve<IStarDataProvider>();
      var a = prov.GetConstellations();

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

    [Test]
    public async Task ShowUniverseAsync()
    {
      var prov = Container.Resolve<IStarDataAsyncProvider>();
      var a = await prov.GetConstellationsAsync();

      Assert.IsTrue(a.Count() > 0);
    }

    [Test]
    public async Task StarDetailMappingAsync()
    {
      var prov = Container.Resolve<IStarDataAsyncProvider>();
      var all = await prov.GetStarsAsync(new Cll.Filters.StarFilter()
      {
        DistanceTo = 100000,
        MagnitudeTo = 100000
      });

      var e = all.GetEnumerator();
      Assert.DoesNotThrowAsync(new AsyncTestDelegate(async () => // It takes a few minutes for all
      {
        while (e.MoveNext())
        {
          var a = await prov.GetStarDetailsAsync(e.Current.Id);
          var b = a.Designation;
        };
      }));
    }
  }
}
