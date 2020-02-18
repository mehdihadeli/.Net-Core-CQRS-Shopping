using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Shopping.Application.CommandHandlers;
using Shopping.Core.Commands;
using Shopping.Core.Events;
using Shopping.Tests.UnitTests.Common;
using Shouldly;
using Xunit;

namespace Shopping.Tests.UnitTests.CommandHandlers.Customer
{
    public class CreateCustomerCommandTests : UnitTestBase
    {
        [Fact]
        public async Task Should_Raise_A_CustomerCreated_Notification_Event_When_Customer_Created()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var sut = new CreateCustomerCommandHandler(ShoppingDbContext, mediatorMock.Object);
            var message = new CreateCustomerCommand {Id = 1};

            // Act
            await sut.Handle(message, default);

            // Assert
            mediatorMock.Verify(m => m.Publish(It.Is<CustomerCreatedEvent>(cc => cc.CustomerId == message.Id),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Create_A_Customer()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var sut = new CreateCustomerCommandHandler(ShoppingDbContext, mediatorMock.Object);
            var message = new CreateCustomerCommand {Id = 1, City = "Tehran", Country = "Iran", Phone = "232323"};

            // Act
            await sut.Handle(message, default);

            //Assert
            var customer = ShoppingDbContext.Customers.FirstOrDefault(q => q.Id == message.Id);
            customer.ShouldNotBeNull();
            customer.Id.ShouldBe(message.Id);
            customer.City.ShouldBe(message.City);
            customer.Country.ShouldBe(message.Country);
            customer.Phone.ShouldBe(message.Phone);
        }
    }
}