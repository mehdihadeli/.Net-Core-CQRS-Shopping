using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Web.Common.Exceptions;
using MediatR;
using Shopping.Core.Commands;
using Shopping.Core.Domains;
using Shopping.Infrastructure.Persistence.Shopping;

namespace Shopping.Application.CommandHandlers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly IShoppingDataContext _context;

        public DeleteCustomerCommandHandler(IShoppingDataContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customers = _context.Set<Core.Domains.Customer>();
            var entity = await customers.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }

            var hasOrders = _context.Set<Order>().Any(o => o.Id == entity.Id);
            if (hasOrders)
            {
                throw new DeleteFailureException(nameof(Customer), request.Id,
                    "There are existing orders associated with this customer.");
            }

            customers.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}