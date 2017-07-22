using SQLite;
using StarMap.Cll.Abstractions;

namespace StarMap.Dal.Providers
{
  public abstract class DatabaseProvider : BaseProvider<SQLiteConnection>
  {
    string _repository;
    public DatabaseProvider(IRepository repository)
    {
      _repository = repository.GetFilePath();
    }

    protected override SQLiteConnection Context => new SQLiteConnection(_repository, SQLiteOpenFlags.ReadOnly);
  }
}
