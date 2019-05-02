namespace StarMap.Cll.Abstractions
{
    public interface IRepository
    {
        /// <summary>
        /// Returns the path to the data file.
        /// </summary>
        string GetFilePath(); // TODO: create another class that has this dependency, that returns the connection.
    }
}
