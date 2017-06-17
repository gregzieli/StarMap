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
    static ISettings AppSettings => CrossSettings.Current;

    #region Setting Constants

    const string GeolocationKey = "loc_key";

    const string FilterKey = "filter_key";

    const string SensorsKey = "sensor_key";

    #endregion

    public static string Geolocation
    {
      get { return AppSettings.GetValueOrDefault<string>(GeolocationKey); }
      set { AppSettings.AddOrUpdateValue(GeolocationKey, value); }
    }

    public static string Filter
    {
      get { return AppSettings.GetValueOrDefault<string>(FilterKey); }
      set { AppSettings.AddOrUpdateValue(FilterKey, value); }
    }

    public static bool SensorsOn
    {
      get { return AppSettings.GetValueOrDefault<bool>(SensorsKey, false); } // now to false, but on release should be true.
      set { AppSettings.AddOrUpdateValue(SensorsKey, value); }
    }
  }
}