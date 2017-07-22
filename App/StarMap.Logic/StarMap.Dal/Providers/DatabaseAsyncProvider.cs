using SQLite;
using StarMap.Cll.Abstractions;

namespace StarMap.Dal.Providers
{
  public abstract class DatabaseAsyncProvider
  {
    string _repository;
    public DatabaseAsyncProvider(IRepository repository)
    {
      _repository = repository.GetFilePath();
    }

    protected SQLiteAsyncConnection Context => new SQLiteAsyncConnection(_repository, SQLiteOpenFlags.ReadOnly);
  }
}
