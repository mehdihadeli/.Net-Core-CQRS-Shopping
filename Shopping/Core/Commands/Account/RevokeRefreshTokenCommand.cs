using System;
using MediatR;

namespace Shopping.Core.Commands
{
    public class RevokeRefreshTokenCommand : IRequest
    {
        public int UserId { get; }
        public string Token { get; }

        public RevokeRefreshTokenCommand(int userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }
}