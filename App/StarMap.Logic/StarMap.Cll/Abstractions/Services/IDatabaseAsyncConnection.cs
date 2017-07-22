using SQLite;

namespace StarMap.Cll.Abstractions
{
  public interface IDatabaseAsyncConnection
  {
    SQLiteAsyncConnection GetConnection();
  }
}
