using MediatR;
using Shopping.Core.ViewModels;

namespace Shopping.Core.Queries
{
    public class GetProductsFileQuery : IRequest<ProductsFileVm>
    {
    }
}
