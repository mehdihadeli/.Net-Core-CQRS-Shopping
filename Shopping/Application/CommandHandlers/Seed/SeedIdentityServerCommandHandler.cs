using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using MediatR;
using Shopping.Core.Commands.Seed;
using Shopping.Infrastructure.Persistence;

namespace Shopping.Application.CommandHandlers
{
    public class SeedIdentityServerCommandHandler:IRequestHandler<SeedIdentityServerCommand>
    {
        private readonly ConfigurationDbContext _context;

        public SeedIdentityServerCommandHandler(ConfigurationDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(SeedIdentityServerCommand request, CancellationToken cancellationToken)
        {
            await  IdentityServerSeedData.SeedAsync(_context);
          
            return Unit.Value;
        }
    }
}