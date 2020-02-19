using StarMap.Core.Abstractions;

namespace StarMap.Cll.Test.Stubs
{
    public class CollectionItem : IUnique
    {
        public int Id { get; private set; }

        public string MyProperty1 { get; set; }

        public string MyProperty2 { get; set; }

        public double MyProperty3 { get; set; }

        public string MyProperty4 { get; set; }

        public string MyProperty5 { get; set; }

        private static int _count;

        public CollectionItem()
        {
            Id = ++_count;
        }
    }
}
