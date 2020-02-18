using MediatR;

namespace Shopping.Core.Commands
{
    public class DeleteEmployeeCommand : IRequest
    {
        public int Id { get; set; }

       
    }
}
