using NUnit.Framework;
using StarMap.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarMap.LogicTest
{
  [TestFixture]
  public class Utils
  {
    [Test]
    public void Normalization()
    {
      Assert.AreEqual(0.5, Normalizer.Normalize(5, 0, 10));
      Assert.AreEqual(0, Normalizer.Normalize(5, 0, 10, -10, 10));
      Assert.AreEqual(0.25, Normalizer.Normalize(1, 0, 4));
      Assert.AreEqual(25, Normalizer.Normalize(1, 0, 4, 0, 100));
      Assert.AreEqual(12.5, Normalizer.Normalize(1, 0, 4, 0, 50));
      
      var aaaa = Normalizer.Normalize(-15, -8, 8, 1.2, 0.5);
    }
  }
}
