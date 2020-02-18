using MediatR;

namespace Shopping.Core.Commands
{
    public class DeleteCustomerCommand : IRequest
    {
        public int Id { get; set; }
    }
}
