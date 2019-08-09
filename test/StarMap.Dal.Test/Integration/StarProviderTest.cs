using Moq;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Filters;
using StarMap.Dal.Providers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using StarMap.Cll.Models.Cosmos;
using StarMap.Cll.Abstractions.Providers;

namespace StarMap.Dal.Test.Integration
{
    public class StarProviderTest
    {
        private readonly Mock<IRepository> _mockRepository = new Mock<IRepository>();

        [Fact]
        public async Task StarDatabaseProvider_Should_GetConstellations()
        {
            // Assign
            var provider = GetProvider();

            // Act
            var constellations = await provider.GetConstellationsAsync();

            //Assert
            Assert.NotEmpty(constellations);
            _mockRepository.Verify(x => x.GetFilePath(), Times.Once);
        }

        [Fact]
        public async Task StarDatabaseProvider_Should_GetStarsDetails()
        {
            // Assign
            var provider = GetProvider();
            const int expectedId = 32263;
            const string expectedDesignation = "Sirius, 9 α Canis Majoris";
            const string expectedLuminosityDescription = "22,8 times brighter";
            const string expectedConstellationName = "Canis Major";

            // Act
            var actualResult = await provider.GetStarDetailsAsync(expectedId);

            // Assert
            _mockRepository.Verify(x => x.GetFilePath(), Times.Once);
            Assert.Equal(expectedDesignation, actualResult.Designation);
            Assert.Equal(expectedLuminosityDescription, actualResult.LuminosityDescription);
            Assert.Equal(expectedConstellationName, actualResult.Constellation.Name);
            Assert.Equal(expectedId, actualResult.Id);
        }

        [Fact]
        public async Task StarDatabaseProvider_Should_GetStars_AlwaysWithSun()
        {
            // Assign
            var provider = GetProvider();

            // Act
            var stars = await provider.GetStarsAsync(new StarFilter
            {
                DesignationQuery = "awd",
                DistanceTo = 901
            });

            // Assert
            var sun = stars.SingleOrDefault(x => x.Id == 0);
            Assert.NotNull(sun);
            Assert.Equal("Sol", sun.Name);
            _mockRepository.Verify(x => x.GetFilePath(), Times.Once);
        }

        [Fact]
        public async Task StarDatabaseProvider_Should_FilterStars()
        {
            // Assign
            var prov = GetProvider();
            var expectedResult = new[]
            {
                new Star { Id = 0, Name = "Sol" },
                new Star { Id = 32263, Name = "Sirius" },
                new Star { Id = 37173, Name = "Procyon" },
                new Star { Id = 71453, Name = "hip 71681, Alp-2" },
                new Star { Id = 71456, Name = "Rigil Kentaurus" }
            };

            // Act
            var actualResult = await prov.GetStarsAsync(new StarFilter()
            {
                DistanceTo = 4,
                MagnitudeTo = 3
            });

            // Assert
            _mockRepository.Verify(x => x.GetFilePath(), Times.Once);
            actualResult.Should().BeEquivalentTo(expectedResult, x => x
                .Including(y => y.Id)
                .Including(y => y.Designation));
        }

        [Fact]
        public async Task StarDatabaseProvider_Should_FilterStarsByDesignationAlso()
        {
            // Assign
            var prov = GetProvider();
            var expectedResult = new[]
            {
                new Star { Id = 0, Name = "Sol" },
                new Star { Id = 37173, Name = "Procyon" },
            };

            // Act
            var actualResult = await prov.GetStarsAsync(new StarFilter()
            {
                DistanceTo = 4,
                MagnitudeTo = 3,
                DesignationQuery = "Proc"
            });

            // Assert
            _mockRepository.Verify(x => x.GetFilePath(), Times.Once);
            actualResult.Should().BeEquivalentTo(expectedResult, x => x
                .Including(y => y.Id)
                .Including(y => y.Name)
                .Including(y => y.Designation));
        }

        private IStarProvider GetProvider()
        {
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\src\StarMap.Dal\Database\Universe.db3");

            _mockRepository.Setup(x => x.GetFilePath())
                .Returns(dbPath);

            return new StarProvider(new ConnectionProvider(_mockRepository.Object));
        }
    }
}
