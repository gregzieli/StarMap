using StarMap.Cll.Abstractions;
using System.IO;

namespace StarMap.Dal.Test.Stubs
{
    public class MockRepository : IRepository
    {
        public string GetFilePath()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"StarMap.Dal\Database\Universe.db3"),
              // For some unknown reason, sometimes the Current Directory is system root.
              absolutePath = @"C:\Root\Dev\Stars\StarMap\App\StarMap.Dal\Database\Universe.db3";

            return File.Exists(path) ? path : absolutePath;
        }
    }
}
