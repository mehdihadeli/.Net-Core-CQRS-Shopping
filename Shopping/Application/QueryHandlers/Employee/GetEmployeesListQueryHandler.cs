﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Dtos;
using Shopping.Core.Queries;
using Shopping.Core.ViewModels;
using Shopping.Infrastructure.Persistence.Shopping;

namespace Shopping.Application.QueryHandlers
{
    public class GetEmployeesListQueryHandler : IRequestHandler<GetEmployeesListQuery, EmployeesListVm>
    {
        private readonly IShoppingDataContext _context;
        private readonly IMapper _mapper;

        public GetEmployeesListQueryHandler(IShoppingDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EmployeesListVm> Handle(GetEmployeesListQuery request, CancellationToken cancellationToken)
        {
            var employees = await _context.Set<Core.Domains.Employee>()
                .ProjectTo<EmployeeLookupDto>(_mapper.ConfigurationProvider)
                .OrderBy(e => e.Name)
                .ToListAsync(cancellationToken);

            var vm = new EmployeesListVm
            {
                Employees = employees
            };
                 
            return vm;
        }
    }
}