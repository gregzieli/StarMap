using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarMap.Core.Utils
{
  public static class Randomizer
  {
    static Random random = new Random();
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string RandomString(int length)
      => new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

    public static int RandomInt(int minVal, int maxVal)
      => random.Next(minVal, maxVal);
  }
}
