using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Web.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Domains;
using Shopping.Core.Queries;
using Shopping.Core.ViewModels;
using Shopping.Infrastructure.Persistence.Shopping;

namespace Shopping.Application.QueryHandlers
{
    public class GetProductDetailQueryHandler : MediatR.IRequestHandler<GetProductDetailQuery, ProductDetailVm>
    {
        private readonly IShoppingDataContext _context;
        private readonly IMapper _mapper;

        public GetProductDetailQueryHandler(IShoppingDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductDetailVm> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
        {
            var vm = await _context.Set<Product>()
                .ProjectTo<ProductDetailVm>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (vm == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            return vm;
        }
    }
}
