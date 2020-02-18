using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Shopping.Core.Dtos.User;
using Shopping.Core.Queries;
using Shopping.Core.Services;

namespace Shopping.Application.QueryHandlers.User
{
    public class FilterUserQueryHandler : IRequestHandler<FilterUserQuery, IEnumerable<UserDetailInfoDto>>
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IMapper _mapper;

        public FilterUserQueryHandler(IUserManagerService userManagerService, IMapper mapper)
        {
            _userManagerService = userManagerService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDetailInfoDto>> Handle(FilterUserQuery request,
            CancellationToken cancellationToken)
        {
            var res = await _userManagerService.GetUsers(request.FirstName, request.LastName,
                request.Email, request.PageIndex, request.PageSize
            );

            return _mapper.Map<IEnumerable<UserDetailInfoDto>>(res);
        }
    }
}