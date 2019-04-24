using Moq;
using StarMap.Cll.Abstractions;
using StarMap.Dal.Providers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StarMap.Dal.Test.Units
{
    public class Database
    {
        [Fact]
        public async Task ShowUniverseAsync()
        {
            // Assign
            var provider = GetProvider();

            // Act
            var constellations = await provider.GetConstellationsAsync();

            //Assert
            Assert.NotEmpty(constellations);
        }

        [Fact]
        public async Task StarDetailMappingAsync()
        {
            var prov = GetProvider();
            var all = await prov.GetStarsAsync(new Cll.Filters.StarFilter()
            {
                DistanceTo = 15,
                MagnitudeTo = 15
            });

            var e = all.GetEnumerator();
            while (e.MoveNext())
            {
                var a = await prov.GetStarDetailsAsync(e.Current.Id);
                var b = a.Designation;
            };
        }

        [Fact]
        public async Task CheckSun()
        {
            // Assign
            var provider = GetProvider();

            // Act
            var foo = await provider.GetStarsAsync(new Cll.Filters.StarFilter
            {
                DesignationQuery = "awd",
                DistanceTo = 901

            });

            // Assert
            Assert.Contains(foo, x => x.Id == 0);
        }

        private IStarDataAsyncProvider GetProvider()
        {
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), @"StarMap.Dal\Database\Universe.db3");
             // For some unknown reason, sometimes the Current Directory is system root.
            var absoluteDbPath = @"C:\Root\Dev\Stars\StarMap\App\StarMap.Dal\Database\Universe.db3";

            var mockRepository = new Mock<IRepository>();
            mockRepository.Setup(x => x.GetFilePath())
                .Returns(File.Exists(dbPath) ? dbPath : absoluteDbPath);

            return new StarDatabaseAsyncProvider(mockRepository.Object);
        }
    }
}
