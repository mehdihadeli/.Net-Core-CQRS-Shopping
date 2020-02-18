using System.Collections.Generic;
using Shopping.Core.Dtos;

namespace Shopping.Core.ViewModels
{
    public class CategoriesListVm
    {
        public IList<CategoryDto> Categories { get; set; }

        public int Count { get; set; }
    }
}
