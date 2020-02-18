using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Shopping.Core.Dtos.User;
using Shopping.Core.Queries;
using Shopping.Core.Services;

namespace Shopping.Application.QueryHandlers.User
{
    public class UserQueryHandler : IRequestHandler<UserQuery, UserDetailInfoDto>
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IMapper _mapper;
        public UserQueryHandler(IUserManagerService userManagerService, IMapper mapper)
        {
            _userManagerService = userManagerService;
            _mapper = mapper;
        }

        public async Task<UserDetailInfoDto> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<UserDetailInfoDto>(await _userManagerService.GetUserById(request.Id));
        }
    }
}
