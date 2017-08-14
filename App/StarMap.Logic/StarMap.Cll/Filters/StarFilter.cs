using StarMap.Core.Abstractions;
using System;
using static StarMap.Cll.Constants.Filters;

namespace StarMap.Cll.Filters
{
  public class StarFilter : ObservableBase, IEquatable<StarFilter>
  {
    private double _distanceTo = DEF_DIST;
    public double DistanceTo
    {
      get => _distanceTo;
      set { SetProperty(ref _distanceTo, value); }
    }

    private double _magnitudeTo = DEF_MAG;
    public double MagnitudeTo
    {
      get => _magnitudeTo;
      set { SetProperty(ref _magnitudeTo, value); }
    }

    private string _designationQuery;
    public string DesignationQuery
    {
      get => _designationQuery;
      set { SetProperty(ref _designationQuery, value); }
    }

    /// <summary>
    /// Resets the filter to its default values.
    /// </summary>
    public void Reset()
    {
      DistanceTo = DEF_DIST;
      MagnitudeTo = DEF_MAG;
      DesignationQuery = default(string);
    }

    public bool Equals(StarFilter other)
    {
      if (other is null) return false;

      return DistanceTo == other.DistanceTo
        && MagnitudeTo == other.MagnitudeTo
        && DesignationQuery == other.DesignationQuery;
    }

    public override bool Equals(object obj)
           => Equals(obj as StarFilter);

    public override int GetHashCode()
        => (DistanceTo + MagnitudeTo + DesignationQuery).GetHashCode();
  }
}
