using System.Collections.Generic;
using Xunit;

namespace StarMap.Cll.Test.Units
{
    public class Doodles
    {
        [Fact]
        public void Bools()
        {
            var a = false;
            Assert.True(a = !a);
            Assert.False(a = !a);
            Assert.True(a = !a);

            var b = true;
            Assert.False(b = !b);
            Assert.True(b = !b);
            Assert.False(b = !b);
            Assert.True(b = !b);
        }

        [Fact]
        public void RemovingFromArray()
        {
            var a = new List<uint> { 1, 2, 3, 4, 5 };
            a.Remove(6546);
        }

        [Fact]
        public void Miscellaneous()
        {
            bool a = true, b = false;
            Assert.True(a ^ b);

            string c = "", d = string.Empty;
            Assert.Equal(c, d);

            var e = default(string);
            Assert.Null(e);

            var f = new List<int> { 1, 2, 3, 4, 5 };
            f.Remove(6546);
        }
    }
}
