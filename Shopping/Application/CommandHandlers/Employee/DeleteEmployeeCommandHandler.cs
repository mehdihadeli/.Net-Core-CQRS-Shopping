using System.Threading;
using System.Threading.Tasks;
using Common.Web.Common.Exceptions;
using MediatR;
using Shopping.Core.Commands;
using Shopping.Core.Domains;
using Shopping.Core.Services;
using Shopping.Infrastructure.Persistence.Shopping;

namespace Shopping.Application.CommandHandlers
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
    {
        private readonly IShoppingDataContext _context;
        private readonly IUserManagerService _userManager;
        private readonly ICurrentUserService _currentUser;

        public DeleteEmployeeCommandHandler(IShoppingDataContext context, IUserManagerService userManager,
            ICurrentUserService currentUser)
        {
            _context = context;
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employees = _context.Set<Employee>();
            var entity = await employees.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Employee), request.Id);
            }

            if (entity.UserId == _currentUser.UserId)
            {
                throw new BadRequestException("Employees cannot delete their own account.");
            }

            if (entity.UserId > 0)
            {
                await _userManager.DeleteUserAsync(entity.UserId);
            }

            // TODO: Update this logic, this will only work if the employee has no associated territories or orders.Emp

            employees.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}