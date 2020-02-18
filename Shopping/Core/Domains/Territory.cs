using System.Collections.Generic;
using Common.Core.Domain;

namespace Shopping.Core.Domains
{
    public class Territory : Entity
    {
        public Territory()
        {
            EmployeeTerritories = new HashSet<EmployeeTerritory>();
        }
        public string TerritoryDescription { get; set; }
        public int RegionId { get; set; }

        public Region Region { get; set; }
        public ICollection<EmployeeTerritory> EmployeeTerritories { get; private set; }
    }
}