using Newtonsoft.Json;
using System;

namespace StarMap.Bll.Managers
{
  public abstract class BaseManager
  {

    protected T Deserialize<T>(string json, bool canThrow = true)
    {
      try
      {
        return JsonConvert.DeserializeObject<T>(json);
      }
      catch (Exception e)
      {
        if (!canThrow)
          return default(T);
        throw new Exception("An error occured upon deserializing an object", e);
      }
    }

    protected T Deserialize<T>(string json, T defValue)
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

    protected string Serialize(object obj)
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
