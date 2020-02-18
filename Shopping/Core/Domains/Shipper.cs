using System.Collections.Generic;
using Common.Core.Domain;

namespace Shopping.Core.Domains
{
    public class Shipper : Entity
    {
        public Shipper()
        {
            Orders = new HashSet<Order>();
        }
        public string CompanyName { get; set; }
        public string Phone { get; set; }

        public ICollection<Order> Orders { get; private set; }
    }
}