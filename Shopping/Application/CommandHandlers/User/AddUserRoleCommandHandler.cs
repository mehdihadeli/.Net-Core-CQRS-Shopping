using System.Threading;
using System.Threading.Tasks;
using Common.EF;
using MediatR;
using Shopping.Core.Commands;
using Shopping.Core.Services;
using Shopping.Infrastructure.Persistence.Identity;

namespace Shopping.Application.CommandHandlers.User
{
    public class AddUserRoleCommandHandler : IRequestHandler<AddUserRoleCommand>
    {
        private readonly IIdentityDataContext _dataContext;
        private readonly IUserManagerService _userManagerService;

        public AddUserRoleCommandHandler(IIdentityDataContext dataContext, IUserManagerService userManagerService)
        {
            _dataContext = dataContext;
            _userManagerService = userManagerService;
        }
        
        public async Task<Unit> Handle(AddUserRoleCommand command, CancellationToken cancellationToken)
        {
            await _userManagerService.AddRole(command.UserId, command.RoleName);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return  Unit.Value;
        }
    }
}