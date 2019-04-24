namespace StarMap.Cll.Abstractions
{
    public interface ISettingsManager
    {
        int Astrolocation { get; set; }

        string Filter { get; set; }

        string Geolocation { get; set; }

        bool SensorsOn { get; set; }
    }
}