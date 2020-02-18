using System;
using MediatR;

namespace Shopping.Core.Events
{
    public class PasswordChangedEvent : INotification
    {
        public PasswordChangedEvent(int userId, string oldPassword, string newPassword)
        {
            UserId = userId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public int UserId { get; }
        public string OldPassword  { get; }
        public string NewPassword { get; }
    }
}