using MediatR;
using Shopping.Core.ViewModels;

namespace Shopping.Core.Queries
{
    public class GetEmployeeDetailQuery : IRequest<EmployeeDetailVm>
    {
        public int Id { get; set; }

    }
}
