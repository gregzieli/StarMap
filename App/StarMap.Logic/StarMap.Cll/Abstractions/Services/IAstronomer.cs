using StarMap.Cll.Models.Cosmos;
using Xamarin.Forms;

namespace StarMap.Cll.Abstractions.Services
{
  public interface IAstronomer
  {
    Color GetColor(string spectralType);

    double GetTemperature(double colorIndex);
  }
}
