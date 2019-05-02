namespace StarMap.Cll.Abstractions.Managers
{
    public interface ISerializationManager
    {
        T Deserialize<T>(string json, bool canThrow = true);

        T Deserialize<T>(string json, T defValue);

        string Serialize(object obj);
    }
}