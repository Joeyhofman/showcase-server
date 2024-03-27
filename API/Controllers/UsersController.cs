using System.Collections;
using System.Security.Claims;
using AngleSharp.Css;
using API.Requests;
using Application.DataTransferObjects.Entities;
using Application.Users.Commands.DeactivateUserCommand;
using Application.Users.Commands.UpdateUserCommand;
using Application.Users.Queries.GetAllUsersQuery;
using Application.Users.Queries.GetUserByEmailQuery;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IMediator _mediator;
        private UserManager<User> _userManager;

        public UsersController(IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        private async Task<User> getLoggedInUser()
        {
            // Get the current user's ClaimsPrincipal
            ClaimsPrincipal userClaimsPrincipal = HttpContext.User;

            // Check if the user is authenticated
            if (!userClaimsPrincipal.Identity.IsAuthenticated)
            {
                return null;
            }

            // Get the user's ID from the claims
            string userId = _userManager.GetUserId(userClaimsPrincipal);

            // Get the user from the UserManager using the ID
            User user = await _userManager.FindByIdAsync(userId);

            // You have the current user in the 'user' variable
            return user;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ICollection<UserDTO>> Get()
        {
            var query = new GetAllUsersQuery();
            var users = await _mediator.Send(query);
            return users;
        }

        [Authorize]
        [HttpGet("role/{role}")]
        public async Task<IActionResult> HasRole(string role)
        {
            var user = await getLoggedInUser();
            var userHasRole = await _userManager.IsInRoleAsync(user, role);
            return Ok(new { Result = userHasRole });
        }

        [Authorize]
        [HttpGet("auth/")]
        public async Task<IActionResult> IsAuthenticated()
        {
            var user = await getLoggedInUser();
            if(user is null)
            {
                return Unauthorized();
            }

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateUserRequest request)
        {
            var getUserByEmailCommand = new GetUserByEmailQuery(request.Email);
            var user = await _mediator.Send(getUserByEmailCommand);

            if(user is null)
            {
                return BadRequest("user with email does not exist");
            }

            user.Email = request.Email;
            user.UserName = request.Username;

            var updateUserCommand = new UpdateUserCommand(
                id,
                user
                );
            var udpatedUser = await _mediator.Send(updateUserCommand);
            if(updateUserCommand is null)
            {
                return BadRequest("could not udpate user");
            }

            return Ok(udpatedUser);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:Guid}")]
        public IActionResult Delete(Guid id)
        {
            var deactivateCommand = new DeactivateUserCommand(id);
            _mediator.Send(deactivateCommand);
            return Ok();
        }
    }
}
