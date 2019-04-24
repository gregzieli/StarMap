using SQLite;
using StarMap.Core.Abstractions;

namespace StarMap.Dal.Database.Contracts
{
    /// <summary>
    /// Represents the Constellation table in the database
    /// </summary>
    public class Constellation : IUnique
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public Constellation() { }
    }
}
