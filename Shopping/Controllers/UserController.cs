using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Common.Validation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.Core.Commands;
using Shopping.Core.Domains;
using Shopping.Core.Dtos.User;
using Shopping.Core.Queries;

namespace Shopping.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="registerUserCommand"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>User fetch URL in headers</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(
            [FromBody] RegisterUserCommand registerUserCommand,
            CancellationToken ct)
        {
            var user = await Mediator.Send(registerUserCommand, ct);
            return CreatedAtRoute("User", new {id = user.Id}, user);
        }

        /// <summary>
        /// Get user profile for the ID
        /// </summary>
        /// <param name="userId">Unique user identifier</param>
        /// <returns>User profile data</returns>
        [HttpGet("{userId}", Name = "User")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDetailInfoDto>> FindUserById([FromRoute] int userId)
        {
           return Ok(await Mediator.Send(new UserQuery(userId)));
        }

        /// <summary>
        /// Query for user profile
        /// </summary>
        /// <param name="filterUserQuery">Query filter values</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>Collection of user profiles</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailInfoDto>>> FindUsers(
            [FromQuery] FilterUserQuery filterUserQuery, CancellationToken ct)
        {
            return Ok(await Mediator.Send(filterUserQuery, ct));
        }

        /// <summary>
        /// Updates user profile details
        /// </summary>
        /// <param name="profileUpdateCommand">User profile details</param>
        /// <returns>Empty OK response</returns>
        [HttpPut("{userId}")]
        [Authorize(Policy = "SameUserOrAdmin")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoCommand profileUpdateCommand)
        {
            await Mediator.Send(profileUpdateCommand);
            return NoContent();
        }

        /// <summary>
        /// Updates user password
        /// </summary>
        /// <param name="passwordUpdateCommand">User password update details</param>
        /// <returns>Empty OK reponse</returns>
        [HttpPut("{userId}/password")]
        [Authorize(Policy = "SameUserOrAdmin")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordCommand passwordUpdateCommand)
        {
            await Mediator.Send(passwordUpdateCommand);
            return NoContent();
        }

        /// <summary>
        /// Get all roles for user
        /// </summary>  
        /// <param name="userId">Unique user identifier</param>
        /// <returns>List of role names user belongs to</returns>
        [HttpGet("{userId}/roles", Name = "Roles")]
        [Authorize(Policy = "SameUserOrAdmin")]
        public virtual async Task<ActionResult<IEnumerable<string>>> GetUserRoles([FromRoute] int userId)
        {
            var data = await Mediator.Send(new UserRolesQuery {UserId = userId});
            return Ok(data);
        }

        /// <summary>
        /// Removes user from the role
        /// </summary>
        /// <param name="userId">Unique user identifier</param>
        /// <param name="roleName">Role name</param>
        /// <returns>No content 204 status code</returns>
        [HttpDelete("{userId}/roles/{roleName}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> RemoveUserRole([FromRoute] int userId, [FromRoute, Required] string roleName)
        {
            await Mediator.Send(new RemoveUserRoleCommand(userId, roleName));
            return NoContent();
        }

        /// <summary>
        /// Add user to the role
        /// </summary>
        /// <param name="userId">Unique user identifier</param>
        /// <param name="roleName">Role name</param>
        /// <returns>No content 204 status code</returns>
        [HttpPut("{userId}/roles/{roleName}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddUserRole([FromRoute] int userId,
            [FromRoute, Required] String roleName)
        {
            await Mediator.Send(new AddUserRoleCommand(userId, roleName));
            return NoContent();
        }
    }
}