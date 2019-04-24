using StarMap.Bll.Helpers;
using Xunit;

namespace StarMap.Bll.Test.Units
{
    public class AstronomerTest
    {
        [Theory]
        [InlineData(0.656, 5757)]
        [InlineData(0.009, 10014)]
        [InlineData(-0.03, 10516)]
        [InlineData(0.795, 5296)]
        public void TemperatureCheck(double colorIndex, double expectedTemperatureKelvin)
        {
            // Assign
            var astronomer = new Astronomer();

            // Act & Assert
            Assert.Equal(expectedTemperatureKelvin, astronomer.GetTemperature(colorIndex));
        }
    }
}
