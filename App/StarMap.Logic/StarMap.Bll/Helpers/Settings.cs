// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace StarMap.Bll.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
    private static ISettings AppSettings => CrossSettings.Current;

    #region Setting Constants

    private const string GeolocationKey = "loc_key";

    #endregion

    public static string Geolocation
    {
      get { return AppSettings.GetValueOrDefault<string>(GeolocationKey); }
      set { AppSettings.AddOrUpdateValue(GeolocationKey, value); }
    }
  }
}