using SQLite;
using StarMap.Cll.Abstractions;

namespace StarMap.Dal.Providers
{
  public abstract class DatabaseAsyncProvider
  {
    IDatabaseAsyncConnection _connection;
    public DatabaseAsyncProvider(IDatabaseAsyncConnection connection)
    {
      _connection = connection;
    }

    protected SQLiteAsyncConnection Context => _connection.GetConnection();
  }
}
