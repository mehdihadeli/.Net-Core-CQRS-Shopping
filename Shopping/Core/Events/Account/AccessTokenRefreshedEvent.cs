using System;
using MediatR;

namespace Shopping.Core.Events
{
    public class AccessTokenRefreshedEvent : INotification
    {
        public AccessTokenRefreshedEvent(int userId, string accessToken, string refreshToken)
        {
            UserId = userId;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public int UserId { get; }
        public string AccessToken { get; }
        public string RefreshToken { get; }
    }
}