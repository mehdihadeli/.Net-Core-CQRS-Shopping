using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Dtos;
using Shopping.Core.Queries;
using Shopping.Core.ViewModels;
using Shopping.Infrastructure.Persistence;

namespace Shopping.Application.QueryHandlers
{
    public class GetCustomersListQueryHandler : IRequestHandler<GetCustomersListQuery, CustomersListVm>
    {
        private readonly IShoppingDataContext _context;
        private readonly IMapper _mapper;

        public GetCustomersListQueryHandler(IShoppingDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CustomersListVm> Handle(GetCustomersListQuery request, CancellationToken cancellationToken)
        {
            var customers = await _context.Set<Core.Domains.Customer>()
                .ProjectTo<CustomerLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var vm = new CustomersListVm
            {
                Customers = customers
            };

            return vm;
        }
    }
}
