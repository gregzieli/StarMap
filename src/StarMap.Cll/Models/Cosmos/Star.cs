namespace StarMap.Cll.Models.Cosmos
{
    public class Star : StarBase
    {
        public int? ConstellationId { get; set; }

        public double AbsoluteMagnitude { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public float RelativeDistance { get; set; }
    }
}
