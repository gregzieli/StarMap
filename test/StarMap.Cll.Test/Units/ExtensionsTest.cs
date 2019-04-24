using FluentAssertions;
using StarMap.Core.Extensions;
using System.Collections.Generic;
using Xunit;

namespace StarMap.Cll.Test.Units
{
    public class ExtensionsTest
    {
        [Fact]
        public void CollectionBasics()
        {
            var foo = new object[] { 3, 1, 5 };
            var boo = new object[3];
            var poo = new object[] { };

            Assert.False(foo.IsNullOrEmpty());
            Assert.False(boo.IsNullOrEmpty());
            Assert.True(boo.IsNullOrEmpty(checkContents: true));
            Assert.True(poo.IsNullOrEmpty());
            Assert.True(poo.IsNullOrEmpty(checkContents: true));

            foo.Clear();
            Assert.True(!foo.IsNullOrEmpty());
            Assert.True(foo.IsNullOrEmpty(checkContents: true));

            var list = new List<string>() { "1", "2", "3" };
            Assert.False(list.IsNullOrEmpty());
            list.Clear();
            Assert.True(list.IsNullOrEmpty());
        }

        [Fact]
        public void Copying()
        {
            var source = new List<float>() { 5, 2, 1, 6, 123, 890 };
            var copyToDestination = new float[6];
            var shallowCopyDestination = new float[6];

            source.CopyTo(copyToDestination, 0);
            source.ShallowCopy(shallowCopyDestination);

            shallowCopyDestination.Should().BeEquivalentTo(copyToDestination);
        }

        [Fact]
        public void Normalization()
        {
            Assert.Equal(0.5, 5.Normalize(0, 10));
            Assert.Equal(0, 5.Normalize(0, 10, -10, 10));
            Assert.Equal(0.25, 1.Normalize(0, 4));
            Assert.Equal(25, 1.Normalize(0, 4, 0, 100));
            Assert.Equal(12.5, 1.Normalize(0, 4, 0, 50));


            var aa = 123.Normalize(12, 986);
            var aaa = 123.Normalize(12f, 986f);
            var bb = 123f.Normalize(12, 986);
            var bbb = 123f.Normalize(12f, 986f);
            var cc = 123.0.Normalize(12, 986);
            var ccc = 123.0.Normalize(12.0, 986.0);
            Assert.True(aa == aaa && aaa == bb && bb == bbb);
            Assert.Equal(cc, ccc);
        }
    }
}
