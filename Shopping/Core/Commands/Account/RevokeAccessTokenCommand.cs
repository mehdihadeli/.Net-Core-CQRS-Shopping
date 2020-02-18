using System;
using MediatR;

namespace Shopping.Core.Commands
{
    public class RevokeAccessTokenCommand : IRequest
    {
        public Guid UserId { get; }
        public string AccessToken { get; }

        public RevokeAccessTokenCommand(Guid userId, string accessToken)
        {
            UserId = userId;
            AccessToken = accessToken;
        }
    }
}