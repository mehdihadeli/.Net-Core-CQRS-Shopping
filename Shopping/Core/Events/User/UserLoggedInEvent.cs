using System;
using MediatR;

namespace Shopping.Core.Events
{
    public class UserLoggedInEvent : INotification
    {
        public UserLoggedInEvent(int id)
        {
            Id = id;
        }

        public int Id { get;  }
    }
}