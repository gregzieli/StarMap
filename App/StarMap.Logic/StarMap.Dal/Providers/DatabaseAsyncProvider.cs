using SQLite;
using StarMap.Cll.Abstractions;
using System;
using System.Threading.Tasks;

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

    /// <summary>
    /// Grants access to the <see cref="SQLite.SQLiteAsyncConnection"/>
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="action">A read action to perform on database through the <see cref="SQLite.SQLiteAsyncConnection"/>.</param>
    /// <returns>The value taken from DB.</returns>
    protected async Task<T> Read<T>(Func<SQLiteAsyncConnection, Task<T>> action)
    {
      // TODO: could make Context private and expose it only by this
      //       makes sense for those situations when in one method I call Context more than once
      //       But I don't see as any major improvement
      try
      {
        return await action(Context).ConfigureAwait(false);
      }
      catch
      {
        throw new Exception("Database access exception");
      }
    }
  }
}
