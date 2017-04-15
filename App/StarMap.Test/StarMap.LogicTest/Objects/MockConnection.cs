using SQLite;
using StarMap.Cll.Abstractions;
using System.IO;

namespace StarMap.LogicTest.Objects
{
  public class MockConnection : IDatabaseConnection
  {    
    //  With Unity, in the SetupFixture, I could register the provider with this dependency, and call the manager in the test.
    public SQLiteConnection GetConnection()
    {
      string path = Path.Combine(Directory.GetCurrentDirectory(), @"StarMap.Dal\Database\Universe.db3"),
        // For some unknown reason, sometimes the Current Directory is system root.
        absolutePath = @"C:\Root\Other\StarMap\App\StarMap.Logic\StarMap.Dal\Database\Universe.db3";

      return new SQLiteConnection(File.Exists(path) ? path : absolutePath);
    }
  }
}
