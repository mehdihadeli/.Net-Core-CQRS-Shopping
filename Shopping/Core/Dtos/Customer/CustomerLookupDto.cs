using AutoMapper;
using Shopping.Core.Domains;
using Shopping.Core.Mappings;

namespace Shopping.Core.Dtos
{
    public class CustomerLookupDto : IMapFrom<Customer>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Customer, CustomerLookupDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.CompanyName));
        }
    }
}
