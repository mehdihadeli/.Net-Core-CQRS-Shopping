using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.Web.Common.Exceptions;
using MediatR;
using Shopping.Core.Domains;
using Shopping.Core.Queries;
using Shopping.Core.ViewModels;
using Shopping.Infrastructure.Persistence;

namespace Shopping.Application.QueryHandlers
{
    public class GetCustomerDetailQueryHandler : IRequestHandler<GetCustomerDetailQuery, CustomerDetailVm>
    {
        private readonly IShoppingDataContext _context;
        private readonly IMapper _mapper;

        public GetCustomerDetailQueryHandler(IShoppingDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<CustomerDetailVm> Handle(GetCustomerDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<Customer>()
                .FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }

            return _mapper.Map<CustomerDetailVm>(entity);
        }
    }
}