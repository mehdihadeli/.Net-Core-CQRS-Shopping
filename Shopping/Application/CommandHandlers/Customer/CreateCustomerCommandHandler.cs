using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Commands;
using Shopping.Core.Events;
using Shopping.Infrastructure.Persistence;

namespace Shopping.Application.CommandHandlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand>
    {
        private readonly IShoppingDataContext _context;
        private readonly IMediator _mediator;

        public CreateCustomerCommandHandler(IShoppingDataContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = new Core.Domains.Customer
            {
                Id = request.Id,
                Address = request.Address,
                City = request.City,
                CompanyName = request.CompanyName,
                ContactName = request.ContactName,
                ContactTitle = request.ContactTitle,
                Country = request.Country,
                Fax = request.Fax,
                Phone = request.Phone,
                PostalCode = request.PostalCode
            };

            _context.Set<Core.Domains.Customer>().Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new CustomerCreatedEvent() { CustomerId = entity.Id }, cancellationToken);

            return Unit.Value;
        }
    }

}