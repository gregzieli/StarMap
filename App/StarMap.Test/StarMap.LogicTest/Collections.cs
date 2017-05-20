using NUnit.Framework;
using StarMap.Core.Models;
using StarMap.LogicTest.Classes;
using System.ComponentModel;
using System.Linq;

namespace StarMap.LogicTest
{
  [TestFixture]
  public class Collections
  {
    [Test] 
    public void ObservantCollections()
    {
      var a = new ElementAwareCollection<Dog>();
      var firstToGo = new Dog();
      a.ElementChanged += ElementChanged;
      firstToGo.PropertyChanged += ReferencedObjectChanged;
      bool refRaised = false, 
        elRaised = false;

      a.Add(new Dog());
      a.Add(new Dog());
      a.Add(firstToGo);
      
      firstToGo.Age = 123;

      Assert.IsTrue(refRaised);
      Assert.IsTrue(elRaised);
      Reset();

      a.Remove(firstToGo);
      firstToGo.Age = 1;

      Assert.IsTrue(refRaised);
      Assert.IsFalse(elRaised);
      Reset();

      a.Add(new Dog());
      a[2].Age = 999;
      Assert.IsFalse(refRaised);
      Assert.IsTrue(elRaised);
      Reset();

      a.FirstOrDefault().Age = 11111;
      Assert.IsFalse(refRaised);
      Assert.IsTrue(elRaised);
      Reset();

      void ReferencedObjectChanged(object sender, PropertyChangedEventArgs e)
      => refRaised = true;

      void ElementChanged(object sender, PropertyChangedEventArgs e)
        => elRaised = true;

      void Reset()
        => refRaised = elRaised = false;
    }    
  }
}
