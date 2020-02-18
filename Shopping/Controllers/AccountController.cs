using System.Threading;
using System.Threading.Tasks;
using Common.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.Core.Commands;
using Shopping.Core.Domains;

namespace Shopping.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="registerUserCommand">Information for registering a new user</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>User fetch URL in headers</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApplicationUser>> RegisterUser([FromBody] RegisterUserCommand registerUserCommand,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await Mediator.Send(registerUserCommand, ct);
            return CreatedAtRoute("Default", new {controller = "User", userId = user.Id}, user);
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="signInUserCommand">Information for authenticating a user</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>JsonWebToken</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<JsonWebToken>> LoginUser([FromBody] SignInUserCommand signInUserCommand,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await Mediator.Send(signInUserCommand, ct);
        }

        /// <summary>
        /// Updates user password
        /// </summary>
        /// <param name="passwordUpdateCommand">User password update details</param>
        /// <returns>Empty OK response</returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordCommand passwordUpdateCommand)
        {
            await Mediator.Send(passwordUpdateCommand);
            return NoContent();
        }
    }
}