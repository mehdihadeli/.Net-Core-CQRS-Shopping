using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Events;

namespace Shopping.Application.EventHandlers
{
    public class UserLoggedInEventHandler : INotificationHandler<UserLoggedInEvent>
    {
        public Task Handle(UserLoggedInEvent notification, CancellationToken cancellationToken)
        {
            //TODO
            return Task.CompletedTask;
        }
    }
}