using Common.Core.Domain;

namespace Shopping.Core.Domains
{
    public class EmployeeTerritory : Entity
    {
        public int EmployeeId { get; set; }
        public int TerritoryId { get; set; }
        public Employee Employee { get; set; }
        public Territory Territory { get; set; }
    }
}