using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shopping.Core.Commands;
using Shopping.Core.Domains;
using Shopping.Core.Events;
using Shopping.Core.Services;
using Shopping.Infrastructure.Persistence.Identity;

namespace Shopping.Application.CommandHandlers.User
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApplicationUser>
    {
        private readonly IIdentityDataContext _dataContext;
        private readonly IMediator _mediator;
        private readonly IUserManagerService _userManagerService;

        private readonly IMapper _mapper;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(IIdentityDataContext dataContext, IMapper mapper,
            ILogger<RegisterUserCommandHandler> logger,
            IMediator mediator, IUserManagerService userManagerService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _userManagerService = userManagerService;
        }

        public async Task<ApplicationUser> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManagerService.CreateUser(new ApplicationUser()
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                UserName = command.UserName,
                Email = command.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            }, command.Password);

            await _dataContext.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(
                new UserRegisteredEvent(id: user.Id, email: user.Email, firstName: user.FirstName,
                    lastName: user.LastName), cancellationToken);
            _logger.Log(LogLevel.Debug, "UserRegistered Event Published.");

            return user;
        }
    }
}