using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Common.Authentication;
using Common.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shopping.Core.Commands;
using Shopping.Core.Events;
using Shopping.Core.Services;

namespace Shopping.Application.CommandHandlers
{
    public class RefreshAccessTokenCommandHandler : IRequestHandler<RefreshAccessTokenCommand, JsonWebToken>
    {
        private readonly IDataContext _dataContext;
        private readonly IJwtService _jwtService;
        private readonly IMediator _mediator;
        private readonly IUserManagerService _userManagerService;
        private readonly ILogger<RefreshAccessTokenCommandHandler> _logger;

        public RefreshAccessTokenCommandHandler(IDataContext dataContext, IJwtService jwtService, IMediator mediator,
            IUserManagerService userManagerService, ILogger<RefreshAccessTokenCommandHandler> logger)
        {
            _dataContext = dataContext;
            _jwtService = jwtService;
            _mediator = mediator;
            _userManagerService = userManagerService;
            _logger = logger;
        }

        public async Task<JsonWebToken> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _dataContext.Set<RefreshToken>()
                .SingleOrDefaultAsync(x => x.Token == request.Token, cancellationToken);

            if (refreshToken == null)
            {
                throw new Exception("Refresh token not found.");
            }

            refreshToken.ValidateRefreshToken();

            var user = await _userManagerService.GetUserById(refreshToken.UserId);
            if (user == null)
            {
                throw new Exception($"User: '{refreshToken.UserId}' not found.");
            }
            
            var newRefreshToken = new RefreshToken(user.Id, 4);

            try
            {
                _dataContext.BeginTransaction();
                _dataContext.Set<RefreshToken>().Remove(refreshToken);
                await _dataContext.Set<RefreshToken>().AddAsync(newRefreshToken, cancellationToken);
                await _dataContext.SaveChangesAsync(cancellationToken);
                _dataContext.Commit();
            }
            catch
            {
                _dataContext.Rollback();
                throw;
            }
            
            var claims = GetCustomClaimsForUser(user.Id);
            var roles = (await _userManagerService.GetRoles(user.Id)).ToImmutableList();
            var jwt = _jwtService.GenerateToken(user.Id.ToString("N"), user.UserName, roles, claims);
            jwt.RefreshToken = newRefreshToken.Token;
            await _mediator.Publish(new AccessTokenRefreshedEvent(user.Id, jwt.AccessToken, newRefreshToken.Token),
                cancellationToken);
            _logger.Log(LogLevel.Debug, "AccessTokenRefreshed Event Published.");
            return jwt;
        }

        Dictionary<string, string> GetCustomClaimsForUser(int userId)
        {
            //Get Custom Claims for User
            return new Dictionary<string, string>();
        }
    }
}