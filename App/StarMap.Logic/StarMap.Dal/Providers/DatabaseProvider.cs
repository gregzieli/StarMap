using SQLite;
using StarMap.Cll.Abstractions;

namespace StarMap.Dal.Providers
{
  public abstract class DatabaseProvider : BaseProvider<SQLiteConnection>
  {
    IDatabaseConnection _connection;
    public DatabaseProvider(IDatabaseConnection connection)
    {
      _connection = connection;
    }

    protected override SQLiteConnection Context => _connection.GetConnection();
  }
}
