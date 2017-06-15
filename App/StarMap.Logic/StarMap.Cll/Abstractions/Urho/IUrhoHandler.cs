using System.Threading.Tasks;
using Urho.Forms;

namespace StarMap.Cll.Abstractions.Urho
{
  public interface IUrhoHandler
  {
    Task GenerateUrho(UrhoSurface surface);

    Task OnUrhoGenerated();
  }
}
