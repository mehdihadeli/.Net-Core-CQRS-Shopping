using MediatR;

namespace Shopping.Core.Commands
{
    public class DeleteCategoryCommand : IRequest
    {
        public int Id { get; set; }
    }
}
