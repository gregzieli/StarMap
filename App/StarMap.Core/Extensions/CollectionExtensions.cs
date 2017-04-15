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
  }
}
