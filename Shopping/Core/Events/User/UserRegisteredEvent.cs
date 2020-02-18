using System;
using MediatR;
using Newtonsoft.Json;

namespace Shopping.Core.Events
{
    public class UserRegisteredEvent : INotification 
    {
        public int Id { get;  }
        public String Email { get; }
        public String FirstName { get;  }
        public String LastName { get; }

        [JsonConstructor]
        public UserRegisteredEvent(int id, String email, String firstName, String lastName)
        {
            this.Id = id;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
        }
    }
}
