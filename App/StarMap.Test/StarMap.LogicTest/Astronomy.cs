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
    public void ColorCheck()
    {
      IAstronomer a = new Astronomer();

      Assert.AreEqual(Color.DarkBlue, a.GetColor("Blaw;kdh 87gb a ajwd   "));
      Assert.AreEqual(Color.Red, a.GetColor("8172tg 1bdi1ud82 fv"));
      Assert.AreEqual(Color.Red, a.GetColor(null));
      Assert.AreEqual(Color.Red, a.GetColor(""));
      Assert.AreEqual(Color.Yellow, a.GetColor("G"));
      Assert.AreEqual(Color.White, a.GetColor("F3"));
      Assert.AreEqual(Color.Yellow, a.GetColor("F4"));
      Assert.AreEqual(Color.Blue, a.GetColor("B3"));
    }

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
