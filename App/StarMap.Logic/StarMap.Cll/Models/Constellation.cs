using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarMap.Cll.Models
{
  public class Constellation
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Abbreviation { get; set; }

    public bool IsSelected { get; set; } = true;

    public List<Star> Stars { get; set; }
  }
}
