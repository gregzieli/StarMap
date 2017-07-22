using NUnit.Framework;
using StarMap.Bll.Helpers;
using StarMap.Cll.Abstractions.Services;
using Xamarin.Forms;

namespace StarMap.LogicTest
{
  [TestFixture]
  public class Astronomy
  {    
    [Test]
    public void TemperatureCheck()
    {
      IAstronomer a = new Astronomer();

      var sun = a.GetTemperature(0.656);
      var sirius = a.GetTemperature(0.009);
      var rigel = a.GetTemperature(-0.03);
      var capella = a.GetTemperature(0.795);
    }
  }
}
