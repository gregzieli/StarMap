using StarMap.Core.Abstractions;

namespace StarMap.Cll.Models.Core
{
    public interface IReferencable : IUnique
    {
        string Designation { get; }
    }
}
