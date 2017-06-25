using NUnit.Framework;
using StarMap.Core.Abstractions;
using StarMap.Core.Models;
using StarMap.LogicTest.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

    [Test]
    public void Casting()
    {
      IList<CollectionItem> iList = new List<CollectionItem>();
      for (int i = 0; i < 10000; i++)
        iList.Add(new CollectionItem());

      var a = new ObservableCollection<CollectionItem>(iList);
      Stopwatch watch = new Stopwatch();

      watch.Start();
      // Ten times slower
      //var foo = a.Where(x => x.MyProperty1 == default(string)).Select(x => x.Id);

      var foo = a.Where(x => x.MyProperty1 == default(string)).Cast<IUnique>();
      watch.Stop();

      System.Console.WriteLine(watch.ElapsedTicks);
    }
  }
}
