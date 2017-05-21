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
  }
}
