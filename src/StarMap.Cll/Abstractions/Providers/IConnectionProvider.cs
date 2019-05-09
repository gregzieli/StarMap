using SQLite;

namespace StarMap.Cll.Abstractions.Providers
{
    public interface IConnectionProvider
    {
        SQLiteAsyncConnection GetConnection();
    }
}
