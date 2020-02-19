using StarMap.Cll.Models.Core;
using System.Linq;
using System.Text;

namespace StarMap.Cll.Models.Cosmos
{
    public abstract class StarBase : IReferencable
    {
        public int Id { get; set; }

        public int? HipparcosId { get; set; }

        public string Name { get; set; }

        public string Bayer { get; set; }

        public string Flamsteed { get; set; }

        public double ParsecDistance { get; set; }

        /// <summary>
        /// Constellation the star belongs to.
        /// </summary>
        public Constellation Constellation { get; set; }

        public double LightYearDistance => ParsecDistance * 3.262;

        private string _designation;
        public virtual string Designation => _designation ?? (_designation = GetDesignation());

        private string GetDesignation()
        {
            var con = Constellation?.Abbreviation;

            if (Name != null)
                return con is null ? Name : $"{Name} ({con})";

            var sb = new StringBuilder();

            if (new[] { Flamsteed, Bayer, con }.Any(x => !string.IsNullOrEmpty(x)))
                sb.AppendFormat("{0}{1} {2}", Flamsteed, Bayer, con);

            if (HipparcosId.HasValue)
            {
                if (sb.Length > 0)
                    sb.Insert(0, $"hip {HipparcosId}, ");
                else
                    sb.Append($"hip {HipparcosId}");
            }

            return sb.ToString().Trim();
        }
    }
}
