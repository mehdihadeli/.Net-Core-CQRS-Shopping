using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Commands.Seed;
using Shopping.Core.Services;
using Shopping.Infrastructure.Persistence;

namespace Shopping.Application.CommandHandlers
{
    public class SeedIdentityCommandHandler : IRequestHandler<SeedIdentityCommand>
    {
        private readonly IIdentityDataContext _context;
        private readonly IUserManagerService _userManager;

        public SeedIdentityCommandHandler(IIdentityDataContext context, IUserManagerService userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(SeedIdentityCommand request, CancellationToken cancellationToken)
        {
            await IdentitySeedData.SeedAsync(_userManager);

            return Unit.Value;
        }
    }
}