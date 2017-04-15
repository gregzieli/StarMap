using SQLite;
using StarMap.Cll.Abstractions;

namespace StarMap.Dal.Providers
{
  public abstract class DatabaseProvider: BaseProvider<SQLiteConnection>
  {
    IDatabaseConnection _connection;
    public DatabaseProvider(IDatabaseConnection connection)
    {
      _connection = connection;
    }

    // This is a C#6 property getter.
    protected override SQLiteConnection Context => _connection.GetConnection();
  }
}
