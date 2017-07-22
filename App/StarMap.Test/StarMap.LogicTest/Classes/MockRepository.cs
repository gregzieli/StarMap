using StarMap.Cll.Abstractions;
using System.IO;

namespace StarMap.LogicTest.Classes
{
  public class MockRepository : IRepository
  {    
    //  With Unity, in the SetupFixture, I could register the provider with this dependency, and call the manager in the test.
    public string GetFilePath()
    {
      string path = Path.Combine(Directory.GetCurrentDirectory(), @"StarMap.Dal\Database\Universe.db3"),
        // For some unknown reason, sometimes the Current Directory is system root.
        absolutePath = @"C:\Root\Git\Private\StarMap\App\StarMap.Logic\StarMap.Dal\Database\Universe.db3";

      return File.Exists(path) ? path : absolutePath;
    }
  }
}
