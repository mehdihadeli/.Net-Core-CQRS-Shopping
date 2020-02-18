using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.Authentication;
using Common.EF;
using MediatR;
using Microsoft.Extensions.Logging;
using Shopping.Core.Commands;
using Shopping.Core.Domains;
using Shopping.Core.Events;
using Shopping.Core.Services;

namespace Shopping.Application.CommandHandlers
{
    public class SignInUserCommandHandler : IRequestHandler<SignInUserCommand, JsonWebToken>
    {
        private readonly IJwtService _jwtService;
        private readonly IUserManagerService _userManagerService;
        private readonly IMapper _mapper;
        private readonly ILogger<SignInUserCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IDataContext _dataContext;

        public SignInUserCommandHandler(
            IUserManagerService userManagerService,
            IJwtService jwtService,
            IMapper mapper, ILogger<SignInUserCommandHandler> logger, IMediator mediator,
            IDataContext dataContext
        )
        {
            _jwtService = jwtService;
            _userManagerService = userManagerService;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _dataContext = dataContext;
        }

        public async Task<JsonWebToken> Handle(SignInUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.EmailOrUserName) || string.IsNullOrWhiteSpace(request.Password))
                throw new Exception("Invalid credentials.");
            ApplicationUser user = await _userManagerService.GetUserByEmailOrUserName(request.EmailOrUserName);

            if (user == null || await _userManagerService.ValidatePasswordAsync(user, request.Password) == false)
            {
                throw new Exception("Invalid credentials.");
            }

            var refreshToken = new RefreshToken(user.Id, 4);
            var roles = (await _userManagerService.GetRoles(user.Id)).ToImmutableList();
            var jwt = _jwtService.GenerateToken(user.Id.ToString(), user.UserName, roles,
                GetCustomClaimsForUser(user.Id));

            jwt.RefreshToken = refreshToken.Token;
            await _dataContext.Set<RefreshToken>().AddAsync(refreshToken, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new UserLoggedInEvent(user.Id), cancellationToken);

            _logger.Log(LogLevel.Debug, "UserLoggedIn Event Published.");

            return jwt;
        }

        private IDictionary<string, string> GetCustomClaimsForUser(int userId)
        {
            //Add custom claims here
            return new Dictionary<string, string>();
        }
    }
}