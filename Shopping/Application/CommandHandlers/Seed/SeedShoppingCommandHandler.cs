using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Commands.Seed;
using Shopping.Infrastructure.Persistence.Shopping;

namespace Shopping.Application.CommandHandlers
{
    public class SeedShoppingCommandHandler: IRequestHandler<SeedShoppingCommand>
    {
        public async Task<Unit> Handle(SeedShoppingCommand request, CancellationToken cancellationToken)
        {
          await  ShoppingSeedData.SeedAsync();
          
          return Unit.Value;
        }
    }
}