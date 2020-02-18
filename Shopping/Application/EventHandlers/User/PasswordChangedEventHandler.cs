using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shopping.Core.Events;

namespace Shopping.Application.EventHandlers
{
    public class PasswordChangedEventHandler : INotificationHandler<PasswordChangedEvent>
    {
        public Task Handle(PasswordChangedEvent notification, CancellationToken cancellationToken)
        {
            //TODO
            return Task.CompletedTask;
        }
    }
}