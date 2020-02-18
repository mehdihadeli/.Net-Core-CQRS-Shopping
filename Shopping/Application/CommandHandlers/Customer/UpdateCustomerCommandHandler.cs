using System.Threading;
using System.Threading.Tasks;
using Common.Web.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Commands;
using Shopping.Infrastructure.Persistence.Shopping;

namespace Shopping.Application.CommandHandlers
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
    {
        private readonly IShoppingDataContext _context;

        public UpdateCustomerCommandHandler(IShoppingDataContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<Core.Domains.Customer>()
                .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Core.Domains.Customer), request.Id);
            }

            entity.Address = request.Address;
            entity.City = request.City;
            entity.CompanyName = request.CompanyName;
            entity.ContactName = request.ContactName;
            entity.ContactTitle = request.ContactTitle;
            entity.Country = request.Country;
            entity.Fax = request.Fax;
            entity.Phone = request.Phone;
            entity.PostalCode = request.PostalCode;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}