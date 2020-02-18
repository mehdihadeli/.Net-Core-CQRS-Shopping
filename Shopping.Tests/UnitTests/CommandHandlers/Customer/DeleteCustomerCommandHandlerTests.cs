using System.Threading;
using System.Threading.Tasks;
using Common.Web.Common.Exceptions;
using Shopping.Application.CommandHandlers;
using Shopping.Core.Commands;
using Shopping.Core.Domains;
using Shopping.Tests.UnitTests.Common;
using Shouldly;
using Xunit;

namespace Shopping.Tests.UnitTests.CommandHandlers.Customer
{
    public class DeleteCustomerCommandHandlerTests : UnitTestBase
    {
        private readonly DeleteCustomerCommandHandler _sut;

        public DeleteCustomerCommandHandlerTests()
        {
            _sut = new DeleteCustomerCommandHandler(ShoppingDbContext);
        }

        protected override async Task LoadShoppingTestDataAsync()
        {
            await ShoppingDbContext.Customers.AddRangeAsync(
                new Core.Domains.Customer {Id = 2, ContactName = "Ralph R. Rao"},
                new Core.Domains.Customer {Id = 3, ContactName = "Edward M. Clay"},
                new Core.Domains.Customer {Id = 4, ContactName = "Wendell S. Graney"});

            await ShoppingDbContext.SaveChangesAsync();
        }


        [Fact]
        public async Task Should_Throws_NotFound_Exception_When_Id_Is_Invalid()
        {
            var InvalidId = 0;
            var command = new DeleteCustomerCommand {Id = InvalidId};

            await Should.ThrowAsync<NotFoundException>(() => _sut.Handle(command, default));
        }

        [Fact]
        public async Task Should_Delete_Customer_With_Valid_Id_And_Zero_Order()
        {
            var validId = 1;

            var command = new DeleteCustomerCommand {Id = validId};
            await _sut.Handle(command, CancellationToken.None);
            var customer = await ShoppingDbContext.Customers.FindAsync(1);

            customer.ShouldBeNull();
        }

        [Fact]
        public async Task Should_Throws_DeleteFailure_Exception_With_Valid_Id_And_Exists_Some_Orders()
        {
            await ShoppingDbContext.Orders.AddAsync(new Order
            {
                Id = 1
            });
             await ShoppingDbContext.SaveChangesAsync();

            var validId = 1;
            var command = new DeleteCustomerCommand {Id = validId};

            await Should.ThrowAsync<DeleteFailureException>(() => _sut.Handle(command, default));
        }
    }
}