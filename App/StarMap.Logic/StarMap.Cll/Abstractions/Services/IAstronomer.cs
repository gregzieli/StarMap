using Xamarin.Forms;

namespace StarMap.Cll.Abstractions.Services
{
  /// <summary>
  /// Contains methods related with describing the stars.
  /// </summary>
  public interface IAstronomer
  {
    /// <summary>
    /// Calculates the color of a star based on its spectral type.
    /// </summary>
    /// <param name="spectralType">Star's spectral type.</param>
    /// <returns>Star color.</returns>
    /// <remarks>
    /// Having this <see cref="Xamarin.Forms.Color"/> dependency is for time purposes only, obviously.
    /// Meaning: when there's time, it should be removed, and either a custom struct returned, 
    /// or even just the RGB values.
    /// </remarks>
    Color GetColor(string spectralType);

    /// <summary>
    /// Calculates temperature [K] of an object based on its B-V color index.
    /// </summary>
    /// <param name="colorIndex">Object's color index, expressed as a difference in its blue and visual magnitudes.</param>
    /// <returns>Object's temperature in degrees Kelvin.</returns>
    double GetTemperature(double colorIndex);
  }
}
