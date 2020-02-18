using System.Collections.Generic;
using Shopping.Core.Dtos;

namespace Shopping.Core.ViewModels
{
    public class ProductsListVm
    {
        public IList<ProductDto> Products { get; set; }

        public bool CreateEnabled { get; set; }
    }
}
