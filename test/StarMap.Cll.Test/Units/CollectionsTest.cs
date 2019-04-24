using Xunit;
using StarMap.Core.Abstractions;
using StarMap.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using StarMap.Cll.Test.Stubs;

namespace StarMap.Cll.Test.Units
{
    public class CollectionsTest
    {
        [Fact]
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

            Assert.True(refRaised);
            Assert.True(elRaised);
            Reset();

            a.Remove(firstToGo);
            firstToGo.Age = 1;

            Assert.True(refRaised);
            Assert.False(elRaised);
            Reset();

            a.Add(new Dog());
            a[2].Age = 999;
            Assert.False(refRaised);
            Assert.True(elRaised);
            Reset();

            a.FirstOrDefault().Age = 11111;
            Assert.False(refRaised);
            Assert.True(elRaised);
            Reset();

            void ReferencedObjectChanged(object sender, PropertyChangedEventArgs e)
            => refRaised = true;

            void ElementChanged(object sender, PropertyChangedEventArgs e)
              => elRaised = true;

            void Reset()
              => refRaised = elRaised = false;
        }

        [Fact]
        public void Casting()
        {
            IList<CollectionItem> iList = new List<CollectionItem>();
            for (var i = 0; i < 10000; i++)
                iList.Add(new CollectionItem());

            var a = new ObservableCollection<CollectionItem>(iList);
            var watch = new Stopwatch();

            watch.Start();
            // Ten times slower
            //var foo = a.Where(x => x.MyProperty1 == default(string)).Select(x => x.Id);

            var foo = a.Where(x => x.MyProperty1 == default).Cast<IUnique>();
            watch.Stop();

            System.Console.WriteLine(watch.ElapsedTicks);
        }
    }
}
