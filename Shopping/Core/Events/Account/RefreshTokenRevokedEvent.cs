using System;
using MediatR;

namespace Shopping.Core.Events
{
    public class RefreshTokenRevokedEvent : INotification
    {
        public RefreshTokenRevokedEvent(int id,int userId, string revokedRefreshToken)
        {
            UserId = userId;
            RevokedRefreshToken = revokedRefreshToken;
            Id = id;
        }

        public int UserId { get; }
        public int Id { get; }
        public string RevokedRefreshToken { get; }
    }
}