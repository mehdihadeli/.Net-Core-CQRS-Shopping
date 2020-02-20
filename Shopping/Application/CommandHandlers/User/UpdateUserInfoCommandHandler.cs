using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Commands;
using Shopping.Core.Services;
using Shopping.Infrastructure.Persistence;

namespace Shopping.Application.CommandHandlers.User
{
    public class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoCommand>
    {
        private readonly IIdentityDataContext _dataContext;
        private readonly IUserManagerService _userManager;

        public UpdateUserInfoCommandHandler(IIdentityDataContext dataContext, IUserManagerService userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(UpdateUserInfoCommand command, CancellationToken cancellationToken)
        {
           var result = await _userManager.UpdateProfile(
                userId: command.Id,
                firstName: command.FirstName,
                lastName: command.LastName
            );

            await _dataContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}