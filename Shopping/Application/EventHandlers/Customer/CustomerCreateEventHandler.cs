using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Events;

namespace Shopping.Application.EventHandlers
{
    public class CustomerCreateEventHandler : INotificationHandler<CustomerCreatedEvent>
    {
        public CustomerCreateEventHandler()
        {
        }

        public  Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}