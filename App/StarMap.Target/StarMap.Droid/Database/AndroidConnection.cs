using SQLite;
using StarMap.Cll.Abstractions;
using StarMap.Droid.Database;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidConnection))]
namespace StarMap.Droid.Database
{
  public class AndroidConnection : IDatabaseConnection
  {
    public AndroidConnection() { }

    public SQLiteConnection GetConnection() => new SQLiteConnection(Connector.GetDatabasePath());
  }
}