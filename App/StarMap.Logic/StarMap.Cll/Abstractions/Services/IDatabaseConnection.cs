using SQLite;

namespace StarMap.Cll.Abstractions
{
  public interface IDatabaseConnection
  {
    SQLiteConnection GetConnection();
  }
}
