using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Queries;
using Shopping.Core.ViewModels;
using Shopping.Infrastructure.Persistence.Shopping;

namespace Shopping.Application.QueryHandlers
{
    public class GetEmployeeDetailQueryHandler : IRequestHandler<GetEmployeeDetailQuery, EmployeeDetailVm>
    {
        private readonly IShoppingDataContext _context;
        private readonly IMapper _mapper;

        public GetEmployeeDetailQueryHandler(IShoppingDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EmployeeDetailVm> Handle(GetEmployeeDetailQuery request, CancellationToken cancellationToken)
        {
            var vm = await _context.Set<Core.Domains.Employee>()
                .Where(e => e.Id == request.Id)
                .ProjectTo<EmployeeDetailVm>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            return vm;
        }
    }
}