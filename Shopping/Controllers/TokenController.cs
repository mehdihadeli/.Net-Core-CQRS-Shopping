using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.Core.Commands;

namespace Shopping.Controllers
{
    public class TokenController : BaseController
    {
        public TokenController(IMediator mediator) : base(mediator)
        {
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshAccessToken(RefreshAccessTokenCommand command, CancellationToken ct)
            => Ok(await Mediator.Send(command, ct));


        [HttpPost]
        public async Task<IActionResult> RevokeAccessToken(RevokeAccessTokenCommand command, CancellationToken ct)
        {
            await Mediator.Send(command, ct);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenCommand command, CancellationToken ct)
        {
            await Mediator.Send(command, ct);

            return NoContent();
        }
    }
}