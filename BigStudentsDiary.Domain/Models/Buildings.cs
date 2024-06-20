using System.ComponentModel.DataAnnotations;

namespace BigStudentsDiary.Domain.Models
{
    public class Buildings
    {
        private Buildings(int buildingId, string building)
        {
            BuildingId = buildingId;
            Building = building;
        }

        public int BuildingId { get; set; }

        public string Building { get; set; } = "";

        public static Buildings Create(int buildingId, string building)
        {
            return new Buildings(buildingId, building);
        }
    }
}