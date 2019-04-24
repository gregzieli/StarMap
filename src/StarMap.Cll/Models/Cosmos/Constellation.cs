using StarMap.Cll.Models.Core.Bindable;
using StarMap.Core.Abstractions;
using System.Collections.Generic;

namespace StarMap.Cll.Models.Cosmos
{
    public class Constellation : MultiSelectable, IUnique
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public List<Star> Stars { get; set; }
    }
}
