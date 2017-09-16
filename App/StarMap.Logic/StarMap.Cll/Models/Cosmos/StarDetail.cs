using System;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace StarMap.Cll.Models.Cosmos
{
  public class StarDetail : StarBase
  {
    public int? HenryDraperId { get; set; }

    public string GlieseId { get; set; }

    /// <summary>
    /// Gets or sets the star's brightness as visible from Earth.
    /// </summary>
    public double ApparentMagnitude { get; set; }

    public double AbsoluteMagnitude { get; set; }

    public string SpectralType { get; set; }

    public double? ColorIndex { get; set; }

    // TODO: lose this dependency!
    public Color Color { get; set; }

    public double Luminosity { get; set; }

    public string LuminosityDescription => $"{Luminosity.ToString("F1")} times {(Luminosity < 1 ? "as bright" : "brighter")}";

    public double? TemperatureKelvin { get; set; }

    public double? TemperatureCelcius => TemperatureKelvin.HasValue ? TemperatureKelvin - 273.15 : null;

    string _designation;
    public override string Designation => _designation ?? (_designation = GetDesignation());

    string GetDesignation()
    {
      var sb = new StringBuilder();
      bool hasBayFlam = !(Flamsteed is null && Bayer is null),
        hasName = Name != null;
      
      if (Flamsteed != null)
        sb.AppendFormat("{0} ", Flamsteed);

      if (Bayer != null)
        sb.AppendFormat("{0} ", MapBayer(Bayer));

      if (Constellation != null && (hasName || hasBayFlam))
        sb.Append(Resources.AppResources.ResourceManager
          .GetString($"{Constellation.Abbreviation}_G"));

      if (hasName)
      {
        if (sb.Length > 0)
          sb.Insert(0, $"{Name}, ");
        else
          sb.Append(Name);
      }
      return sb.ToString().Trim();
    }

    static Regex _bayerRx = new Regex(@"(\w+)(-?\d?)");
    string MapBayer(string bayer)
    {
      Match match = _bayerRx.Match(bayer);
      if (!match.Success)
        return null;

      Func<string, string> convert = greek => $"{greek}{match.Groups[2].Value}";

      switch (match.Groups[1].Value)
      {
        case "Alp":
          return convert("α");
        case "Bet":
          return convert("β");
        case "Gam":
          return convert("γ");
        case "Del":
          return convert("δ");
        case "Eps":
          return convert("ε");
        case "Zet":
          return convert("ζ");
        case "Eta":
          return convert("η");
        case "The":
          return convert("θ");
        case "Iot":
          return convert("ι");
        case "Kap":
          return convert("κ");
        case "Lam":
          return convert("λ");
        case "Mu":
          return convert("μ");
        case "Nu":
          return convert("ν");
        case "Xi":
          return convert("ξ");
        case "Omi":
          return convert("ο");
        case "Pi":
          return convert("π");
        case "Rho":
          return convert("ρ");
        case "Sig":
          return convert("σ");
        case "Tau":
          return convert("τ");
        case "Ups":
          return convert("υ");
        case "Phi":
          return convert("φ");
        case "Chi":
          return convert("χ");
        case "Psi":
          return convert("ψ");
        case "Ome":
          return convert("ω");
        default:
          throw new NotSupportedException("No such letter");
      }
    }
  }
}
