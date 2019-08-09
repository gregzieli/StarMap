namespace StarMap.Core.Abstractions
{
    /// <summary>
    /// Represents any object that has an Id of an integer type.
    /// </summary>
    public interface IUnique
    {
        /// <summary>
        /// Gets the id of this IUnique
        /// </summary>
        int Id { get; }
    }
}
