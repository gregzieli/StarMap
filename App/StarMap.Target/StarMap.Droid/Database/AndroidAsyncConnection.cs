using SQLite;
using StarMap.Cll.Abstractions;
using StarMap.Droid.Database;
using System.Security;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidAsyncConnection))]
namespace StarMap.Droid.Database
{
  public class AndroidAsyncConnection : IDatabaseAsyncConnection
  {
    public AndroidAsyncConnection() { }

    [SecurityCritical] // suggested by Code Analysis
    public SQLiteAsyncConnection GetConnection() => new SQLiteAsyncConnection(Connector.GetDatabasePath());
  }
}