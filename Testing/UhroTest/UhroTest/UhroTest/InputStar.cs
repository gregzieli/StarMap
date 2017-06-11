using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UhroTest
{
  public class InputStar
  {
    public double X { get; set; }

    public double Y { get; set; }

    public double Z { get; set; }

    public string Name { get; set; }

    public double ApparentMagnitude { get; set; }

    public InputStar(double x, double y, double z, double mag, string name = null)
    {
      X = x;
      Y = y;
      Z = z;
      ApparentMagnitude = mag;
      Name = name;
    }
  }
}
