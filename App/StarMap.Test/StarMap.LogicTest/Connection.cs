using Microsoft.Practices.Unity;
using NUnit.Framework;
using StarMap.Cll.Abstractions;
using StarMap.Common.Test;
using System.Linq;

namespace StarMap.LogicTest
{
  [TestFixture]
  public class Connection : SetupFixture
  {
    [Test]
    public void ShowUniverse()
    {
      var manager = Container.Resolve<IStarManager>();
      var a = manager.GetConstellations();

      Assert.IsTrue(a.Count() > 0);
    }
  }
}
