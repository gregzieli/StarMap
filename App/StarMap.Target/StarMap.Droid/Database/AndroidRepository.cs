using StarMap.Cll.Abstractions;
using StarMap.Droid.Database;
using System.Security;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidRepository))]
namespace StarMap.Droid.Database
{
  public class AndroidRepository : IRepository
  {
    public AndroidRepository() { }

    [SecurityCritical] // suggested by Code Analysis
    public string GetFilePath() => Connector.GetDatabasePath();
  }
}