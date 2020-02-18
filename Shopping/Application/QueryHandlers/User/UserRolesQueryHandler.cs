using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Queries;
using Shopping.Core.Services;

namespace Shopping.Application.QueryHandlers.User
{
    public class UserRolesQueryHandler : IRequestHandler<UserRolesQuery, IEnumerable<string>>
    {
        private readonly IUserManagerService _userManagerService;

        public UserRolesQueryHandler(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;
        }

        public Task<IEnumerable<string>> Handle(UserRolesQuery request, CancellationToken cancellationToken)
        {
            return _userManagerService.GetRoles(request.UserId);
        }
    }
}