using SQLite;
using StarMap.Core.Abstractions;

namespace StarMap.Dal.Database.Contracts
{
    /// <summary>
    /// Reprsents the Star table in the database
    /// </summary>
    public class Star : IUnique
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        [Column("hip")]
        public int? HipparcosId { get; set; }

        [Column("hd")]
        public int? HenryDraperId { get; set; }

        [Column("gl")]
        public string GlieseId { get; set; }

        [Column("proper")]
        public string ProperName { get; set; }

        [Column("ra"), NotNull]
        public double RightAscension { get; set; }

        [Column("dec"), NotNull]
        public double Declination { get; set; }

        [Column("dist"), NotNull]
        public double ParsecDistance { get; set; }

        [Column("mag"), NotNull]
        public double ApparentMagnitude { get; set; }

        [Column("absmag"), NotNull]
        public double AbsoluteMagnitude { get; set; }

        [Column("spect")]
        public string SpectralType { get; set; }

        [Column("x")]
        public float X { get; set; }

        [Column("y")]
        public float Y { get; set; }

        [Column("z")]
        public float Z { get; set; }

        [Column("rarad")]
        public double RightAscensionRad { get; set; }

        [Column("decrad")]
        public double DeclinationRad { get; set; }

        [Column("bayer")]
        public string BayerName { get; set; }

        [Column("flam")]
        public string FlamsteedName { get; set; }

        [Column("con")]
        public int? ConstellationId { get; set; }

        [Column("base")]
        public string Base { get; set; }

        [Column("lum")]
        public double Luminosity { get; set; }

        [Column("ci")]
        public double? ColorIndex { get; set; }

        [Column("var")]
        public string Var { get; set; }

        [Column("var_min")]
        public double? VarMin { get; set; }

        [Column("var_max")]
        public double? VarMax { get; set; }

        public Constellation Constellation { get; set; }

        public Star() { }
    }
}
