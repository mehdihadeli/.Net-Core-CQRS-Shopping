using MediatR;
using Shopping.Core.ViewModels;

namespace Shopping.Core.Queries
{
    public class GetCustomerDetailQuery : IRequest<CustomerDetailVm>
    {
        public int Id { get; set; }
    }
}
