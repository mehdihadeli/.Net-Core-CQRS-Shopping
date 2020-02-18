using System.Collections.Generic;
using Common.Core.Domain;

namespace Shopping.Core.Domains
{
    public class Region : Entity
    {
        public Region()
        {
            Territories = new HashSet<Territory>();
        }

        public string RegionDescription { get; set; }
        public ICollection<Territory> Territories { get; private set; }
    }
}