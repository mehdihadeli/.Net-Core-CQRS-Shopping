using System.Collections.Generic;
using Common.Core.Domain;

namespace Shopping.Core.Domains
{
    public class Category : Entity
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }

        public ICollection<Product> Products { get; private set; }
    }
}