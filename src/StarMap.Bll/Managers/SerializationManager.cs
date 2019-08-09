using Newtonsoft.Json;
using StarMap.Cll.Abstractions.Managers;
using System;

namespace StarMap.Bll.Managers
{
    public class SerializationManager : ISerializationManager
    {
        public T Deserialize<T>(string json, bool canThrow = true)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                if (!canThrow)
                    return default;
                throw new Exception("An error occured upon deserializing an object", e);
            }
        }

        public T Deserialize<T>(string json, T defValue)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                return defValue;
            }
        }

        public string Serialize(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception e)
            {
                throw new Exception("An error occured upon serializing an object", e);
            }
        }
    }
}
