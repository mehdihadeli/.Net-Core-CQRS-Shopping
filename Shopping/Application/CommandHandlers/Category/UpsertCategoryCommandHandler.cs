using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Commands;
using Shopping.Core.Domains;
using Shopping.Infrastructure.Persistence;

namespace Shopping.Application.CommandHandlers
{
    public class UpsertCategoryCommandHandler : IRequestHandler<UpsertCategoryCommand, int>
    {
        private readonly IShoppingDataContext _context;

        public UpsertCategoryCommandHandler(IShoppingDataContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpsertCategoryCommand request, CancellationToken cancellationToken)
        {
            Category entity;
            var categories = _context.Set<Category>();
            if (request.Id.HasValue)
            {
                entity = await categories.FindAsync(request.Id.Value);
            }
            else
            {
                entity = new Category();

                categories.Add(entity);
            }

            entity.CategoryName = request.Name;
            entity.Description = request.Description;
            entity.Picture = request.Picture;

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}