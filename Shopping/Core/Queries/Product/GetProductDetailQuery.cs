using MediatR;
using Shopping.Core.ViewModels;

namespace Shopping.Core.Queries
{
    public class GetProductDetailQuery : IRequest<ProductDetailVm>
    {
        public int Id { get; set; }
    }
}
