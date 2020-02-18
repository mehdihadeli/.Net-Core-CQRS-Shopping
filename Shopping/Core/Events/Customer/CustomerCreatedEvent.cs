using MediatR;

namespace Shopping.Core.Events
{
    public class CustomerCreatedEvent : INotification
    {
        public int CustomerId { get; set; }
    }
}
