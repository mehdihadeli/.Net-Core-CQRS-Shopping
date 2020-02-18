using System.Collections.Generic;
using Shopping.Core.Dtos;

namespace Shopping.Core.ViewModels
{
    public class CustomersListVm
    {
        public IList<CustomerLookupDto> Customers { get; set; }
    }
}
