using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Commands;
using Shopping.Core.Domains;
using Shopping.Infrastructure.Persistence.Shopping;

namespace Shopping.Application.CommandHandlers
{
    public class UpsertEmployeeCommandHandler : IRequestHandler<UpsertEmployeeCommand, int>
    {
        private readonly IShoppingDataContext _context;

        public UpsertEmployeeCommandHandler(IShoppingDataContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpsertEmployeeCommand request, CancellationToken cancellationToken)
        {
            Employee entity;
            var employees = _context.Set<Employee>();
            
            if (request.Id.HasValue)
            {
                entity = await employees.FindAsync(request.Id.Value);
            }
            else
            {
                entity = new Employee();

                employees.Add(entity);
            }

            entity.TitleOfCourtesy = request.Title;
            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.BirthDate = request.BirthDate;
            entity.Address = request.Address;
            entity.City = request.City;
            entity.Region = request.Region;
            entity.PostalCode = request.PostalCode;
            entity.Country = request.Country;
            entity.HomePhone = request.HomePhone;
            entity.Title = request.Position;
            entity.Extension = request.Extension;
            entity.HireDate = request.HireDate;
            entity.Notes = request.Notes;
            entity.Photo = request.Photo;
            entity.ReportsTo = request.ManagerId;

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}