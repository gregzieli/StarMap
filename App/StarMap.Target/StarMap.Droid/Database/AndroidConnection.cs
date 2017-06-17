using SQLite;
using StarMap.Cll.Abstractions;
using StarMap.Droid.Database;
using System.Security;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidConnection))]
namespace StarMap.Droid.Database
{
  public class AndroidConnection : IDatabaseConnection
  {
    public AndroidConnection() { }

    [SecurityCritical] // suggested by Code Analysis
    public SQLiteConnection GetConnection() => new SQLiteConnection(Connector.GetDatabasePath());
  }
}