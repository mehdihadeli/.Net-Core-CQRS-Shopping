using Common.Authentication;
using MediatR;
using Newtonsoft.Json;

namespace Shopping.Core.Commands
{
    public class SignInUserCommand : IRequest<JsonWebToken>
    {
        public string Password { get; }
        public string EmailOrUserName { get; }

        [JsonConstructor]
        public SignInUserCommand(string password, string emailOrUserName)
        {
            Password = password;
            EmailOrUserName = emailOrUserName;
        }
    }
}