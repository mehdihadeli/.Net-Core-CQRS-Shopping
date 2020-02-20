using System.Linq;
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
    public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, ProductsListVm>
    {
        private readonly IShoppingDataContext _context;
        private readonly IMapper _mapper;

        public GetProductsListQueryHandler(IShoppingDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductsListVm> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Set<Core.Domains.Product>()
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .OrderBy(p => p.ProductName)
                .ToListAsync(cancellationToken);

            var vm = new ProductsListVm
            {
                Products = products,
                CreateEnabled = true // TODO: Set based on user permissions.
            };

            return vm;
        }
    }
}
