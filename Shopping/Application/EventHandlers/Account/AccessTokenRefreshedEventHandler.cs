using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Events;

namespace Shopping.Application.EventHandlers
{
    public class AccessTokenRefreshedEventHandler: INotificationHandler<AccessTokenRefreshedEvent>
    {
        public Task Handle(AccessTokenRefreshedEvent notification, CancellationToken cancellationToken)
        { 
            //TODO
            return Task.CompletedTask;
        }
    }
}