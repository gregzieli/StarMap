using StarMap.Cll.Abstractions;
using Xamarin.Essentials;

namespace StarMap.Bll.Managers
{
    public class SettingsManager : ISettingsManager
    {
        public string Geolocation
        {
            get => Preferences.Get(nameof(Geolocation), default(string));
            set => Preferences.Set(nameof(Geolocation), value);
        }

        public string Filter
        {
            get => Preferences.Get(nameof(Filter), default(string));
            set => Preferences.Set(nameof(Filter), value);
        }

        public int Astrolocation
        {
            get => Preferences.Get(nameof(Astrolocation), 0); // 0 is Sol
            set => Preferences.Set(nameof(Astrolocation), value);
        }

        public bool SensorsOn
        {
            get => Preferences.Get(nameof(SensorsOn), false);
            set => Preferences.Set(nameof(SensorsOn), value);
        }
    }
}