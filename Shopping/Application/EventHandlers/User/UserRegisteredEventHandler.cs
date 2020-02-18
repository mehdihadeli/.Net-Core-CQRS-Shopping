using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Events;

namespace Shopping.Application.EventHandlers
{
    public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
    {
        public Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            //TODO
            return Task.CompletedTask;
        }
    }
}