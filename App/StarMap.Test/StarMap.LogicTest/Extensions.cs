using StarMap.Core.Extensions;
using NUnit.Framework;
using System.Collections.Generic;

namespace StarMap.LogicTest
{
  [TestFixture]
  public class Extensions
  {
    [Test]
    public void CollectionBasics()
    {
      var foo = new object[] { 3, 1, 5 };
      var boo = new object[3];
      var poo = new object[] { };

      Assert.IsFalse(foo.IsNullOrEmpty());
      Assert.IsFalse(boo.IsNullOrEmpty());
      Assert.IsTrue(boo.IsNullOrEmpty(checkContents: true));
      Assert.IsTrue(poo.IsNullOrEmpty());
      Assert.IsTrue(poo.IsNullOrEmpty(checkContents: true));

      foo.Clear();
      Assert.IsTrue(!foo.IsNullOrEmpty());
      Assert.IsTrue(foo.IsNullOrEmpty(checkContents: true));

      var list = new List<string>() { "1", "2", "3" };
      Assert.IsFalse(list.IsNullOrEmpty());
      list.Clear();
      Assert.IsTrue(list.IsNullOrEmpty());
    }

    [Test]
    public void Copying()
    {
      IList<float> a = new List<float>() { 5, 2, 1, 6, 123, 890 };
      float[] dest = new float[6];

      a.CopyTo(dest, 0);
      a.ShallowCopy(dest);
    }

    [Test]
    public void Normalization()
    {
      Assert.AreEqual(0.5, 5.Normalize(0, 10));
      Assert.AreEqual(0, 5.Normalize(0, 10, -10, 10));
      Assert.AreEqual(0.25, 1.Normalize(0, 4));
      Assert.AreEqual(25, 1.Normalize(0, 4, 0, 100));
      Assert.AreEqual(12.5, 1.Normalize(0, 4, 0, 50));

      var a = -15.1.Normalize(-8, 8, 1.2, 0.5);
      var b = -15.1.Normalize(-8, 8, 1.2, 0.5);
      Assert.AreEqual(a, b);

      var aa = 123.Normalize(12, 986);
      var aaa = 123.Normalize(12f, 986f);
      var bb = 123f.Normalize(12, 986);
      var bbb = 123f.Normalize(12f, 986f);
      var cc = 123.0.Normalize(12, 986);
      var ccc = 123.0.Normalize(12.0, 986.0);
      Assert.IsTrue(aa == aaa && aaa == bb && bb == bbb);
      Assert.AreEqual(cc, ccc);
    }
  }
}
