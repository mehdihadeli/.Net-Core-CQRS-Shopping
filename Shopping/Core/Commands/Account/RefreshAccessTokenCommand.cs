using Common.Authentication;
using MediatR;

namespace Shopping.Core.Commands
{
    public class RefreshAccessTokenCommand : IRequest<JsonWebToken>
    {
        public string Token { get; }

        public RefreshAccessTokenCommand(string token)
        {
            Token = token;
        }
    }
}