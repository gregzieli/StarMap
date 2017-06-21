using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarMap.LogicTest
{
  [TestFixture]
  public class Doodles
  {
    [Test]
    public void Bools()
    {
      bool a = false;
      Assert.IsTrue(a = !a);
      Assert.IsFalse(a = !a);
      Assert.IsTrue(a = !a);

      bool b = true;
      Assert.IsFalse(b = !b);
      Assert.IsTrue(b = !b);
      Assert.IsFalse(b = !b);
      Assert.IsTrue(b = !b);
    }

    [Test]
    public void RemovingFromArray()
    {
      List<uint> a = new List<uint>{ 1, 2, 3, 4, 5 };
      Assert.DoesNotThrow(new TestDelegate(() => a.Remove(6546)));
    }
  }
}
