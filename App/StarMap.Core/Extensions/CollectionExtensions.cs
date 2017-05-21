using System;
using System.Collections.Generic;
using System.Linq;

namespace StarMap.Core.Extensions
{
  public static class CollectionExtensions
  {
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source, bool checkContents = false)
    {
      return source == null || !source.Any() || (checkContents && source.All(x => x == null));
    }

    public static bool IsNullOrEmpty<T>(this T[] source, bool checkContents = false)
    {
      return source == null || source.Length == 0 || (checkContents && source.All(x => x == null));
    }

    public static void Clear<T>(this T[] source)
    {
      Array.Clear(source, 0, source.Length);
    }

    // I know there is ICollection.CopyTo, but this is just in case that one is slower.
    public static void ShallowCopy<T>(this IList<T> source, T[] destination)
    {
      for (int i = 0; i < source.Count; i++)
        destination[i] = source[i];
    }

    public static void ShallowCopy<T>(this IEnumerable<T> source, T[] destination)
    {
      var enumerator = source.GetEnumerator();
      int i = 0;
      while (enumerator.MoveNext())
        destination[i++] = enumerator.Current;
    }
  }
}
