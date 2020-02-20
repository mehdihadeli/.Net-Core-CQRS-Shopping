using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Shopping.Core.Commands;
using Shopping.Core.Events;
using Shopping.Core.Services;
using Shopping.Infrastructure.Persistence;

namespace Shopping.Application.CommandHandlers.User
{
    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand>
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IIdentityDataContext _dataContext;
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateUserPasswordCommandHandler> _logger;

        public UpdateUserPasswordCommandHandler(IUserManagerService userManagerService, IIdentityDataContext dataContext,
            IMediator mediator, ILogger<UpdateUserPasswordCommandHandler> logger)
        {
            _userManagerService = userManagerService;
            _dataContext = dataContext;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            await _userManagerService.UpdatePassword(request.UserId, request.OldPassword, request.NewPassword);
            await _dataContext.SaveChangesAsync(cancellationToken);
            await _mediator.Publish(new PasswordChangedEvent(request.UserId, request.OldPassword, request.NewPassword),
                cancellationToken);
            _logger.Log(LogLevel.Debug, "PasswordChanged Event Published.");
            return Unit.Value;
        }
    }
}