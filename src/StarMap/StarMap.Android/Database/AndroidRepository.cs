using StarMap.Cll.Abstractions;
using StarMap.Droid.Database;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidRepository))]
namespace StarMap.Droid.Database
{
    public class AndroidRepository : IRepository
    {
        public string GetFilePath() => Connector.GetDatabasePath();
    }
}