using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Authentication;
using Common.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shopping.Core.Commands;
using Shopping.Core.Events;

namespace Shopping.Application.CommandHandlers
{
    public class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly IMediator _mediator;
        private readonly ILogger<RevokeRefreshTokenCommandHandler> _logger;

        public RevokeRefreshTokenCommandHandler(IDataContext dataContext, IMediator mediator,
            ILogger<RevokeRefreshTokenCommandHandler> logger)
        {
            _dataContext = dataContext;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _dataContext.Set<RefreshToken>()
                .SingleOrDefaultAsync(x => x.Token == request.Token && x.UserId == request.UserId, cancellationToken);

            if (refreshToken == null)
            {
                throw new Exception("Refresh token was not found.");
            }

            refreshToken.Revoke();
            _dataContext.Set<RefreshToken>().Update(refreshToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
            await _mediator.Publish(
                new RefreshTokenRevokedEvent(refreshToken.Id, refreshToken.UserId, refreshToken.Token),
                cancellationToken);
            _logger.Log(LogLevel.Debug, "RefreshTokenRevoked Event Published.");
            return Unit.Value;
        }
    }
}