using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarMap.Cll.Filters
{
  public class StarFilter
  {
    public int[] ConstellationsIds { get; set; }

    public double? MaxDistance { get; set; }

    public double? MaxMagnitude { get; set; }

    public int? Limit { get; set; } = 1000;

    public string DesignationQuery { get; set; }
  }
}
