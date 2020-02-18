using System.Threading;
using System.Threading.Tasks;
using Common.Web.Common.Exceptions;
using MediatR;
using Shopping.Core.Commands;
using Shopping.Infrastructure.Persistence.Shopping;

namespace Shopping.Application.CommandHandlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IShoppingDataContext _context;

        public DeleteCategoryCommandHandler(IShoppingDataContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<Core.Domains.Category>()
                .FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Core.Domains.Category), request.Id);
            }

            _context.Set<Core.Domains.Category>().Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}