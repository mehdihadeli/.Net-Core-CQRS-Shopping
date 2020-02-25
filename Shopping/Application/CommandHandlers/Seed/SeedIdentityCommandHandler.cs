using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shopping.Core.Commands.Seed;
using Shopping.Core.Domains;
using Shopping.Core.Services;
using Shopping.Infrastructure.Persistence;

namespace Shopping.Application.CommandHandlers
{
    public class SeedIdentityCommandHandler : IRequestHandler<SeedIdentityCommand>
    {
        private readonly IIdentityDataContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public SeedIdentityCommandHandler(IIdentityDataContext context, UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Unit> Handle(SeedIdentityCommand request, CancellationToken cancellationToken)
        {
            await IdentitySeedData.SeedAsync(_userManager, _roleManager);

            return Unit.Value;
        }
    }
}