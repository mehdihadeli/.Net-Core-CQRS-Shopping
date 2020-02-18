using AutoMapper;
using Shopping.Core.Mappings;

namespace Shopping.Core.Dtos
{
    public class ProductRecordDto : IMapFrom<Domains.Product>
    {
        public string Category { get; set; }

        public string Name { get; set; }

        public decimal? UnitPrice { get; set; }

        public bool Discontinued { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domains.Product, ProductRecordDto>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.ProductName))
                .ForMember(d => d.Category, opt => opt.MapFrom(s => s.Category != null ? s.Category.CategoryName : string.Empty));
        }
    }
}
