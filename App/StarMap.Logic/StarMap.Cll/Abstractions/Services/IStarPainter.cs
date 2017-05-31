using StarMap.Cll.Models.Cosmos;
using Xamarin.Forms;

namespace StarMap.Cll.Abstractions.Services
{
  public interface IStarPainter
  {
    Color GetColor(StarDetail star);
  }
}
