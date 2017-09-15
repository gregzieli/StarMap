using System.Threading.Tasks;
using Urho.Forms;

namespace StarMap.Cll.Abstractions.Urho
{
  /// <summary>
  /// Contains general UrhoSharp methods
  /// </summary>
  public interface IUrhoHandler
  {
    Task GenerateUrho(UrhoSurface surface);

    void OnUrhoGenerated();
  }
}
