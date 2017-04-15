using System;
using Android.App;
using System.IO;
using static StarMap.Cll.Constants.Names;

namespace StarMap.Droid.Database
{
  public static class Connector
  {
    public static string GetDatabasePath()
    {
      var sandbox = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
      return Path.Combine(sandbox, DATABASE_NAME);
    }

    public static void CheckDatabase()
    {
      string dbPath = GetDatabasePath();
      if (File.Exists(dbPath))
        return;

      try
      {
        CopyDatabaseFromAssets(dbPath);
      }
      catch (Exception e)
      {
        // TODO
        throw;
      }
    }

    private static void CopyDatabaseFromAssets(string path)
    {
      using (var inStream = Application.Context.Assets.Open(DATABASE_NAME))
      using (var outStream = new FileStream(path, FileMode.Create, FileAccess.Write))
      {
        byte[] buffer = new byte[2048];

        int bytesRead;
        while ((bytesRead = inStream.Read(buffer, 0, buffer.Length)) > 0)
        {
          outStream.Write(buffer, 0, bytesRead);
        }
      }
    }
  }
}