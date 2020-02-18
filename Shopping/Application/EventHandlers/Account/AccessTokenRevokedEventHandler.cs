using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Events;

namespace Shopping.Application.EventHandlers
{
    public class AccessTokenRevokedEventHandler: INotificationHandler<AccessTokenRevokedEvent>
    {
        public Task Handle(AccessTokenRevokedEvent notification, CancellationToken cancellationToken)
        {
            //TODO
            return Task.CompletedTask;
        }
    }
}