using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Commands;
using Shopping.Core.Services;
using Shopping.Infrastructure.Persistence.Identity;

namespace Shopping.Application.CommandHandlers.User
{
    public class RemoveUserRoleCommandHandler : IRequestHandler<RemoveUserRoleCommand>
    {
        private readonly IIdentityDataContext _dataContext;
        private readonly IUserManagerService _userManagerService;
        
        public RemoveUserRoleCommandHandler(IIdentityDataContext dataContext,IUserManagerService userManagerService)
        {
            _dataContext = dataContext;
            _userManagerService = userManagerService;
        }
        
        public async Task<Unit> Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
        {
            await _userManagerService.RemoveRole(request.UserId, request.RoleName);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
