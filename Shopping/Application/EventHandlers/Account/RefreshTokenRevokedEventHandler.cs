using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Events;

namespace Shopping.Application.EventHandlers
{
    public class RefreshTokenRevokedEventHandler:INotificationHandler<RefreshTokenRevokedEvent>
    {
        public Task Handle(RefreshTokenRevokedEvent notification, CancellationToken cancellationToken)
        {
            //TODO
            return Task.CompletedTask;
        }
    }
}